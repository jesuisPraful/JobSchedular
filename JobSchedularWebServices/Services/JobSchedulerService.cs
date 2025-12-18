using JobSchedularDAL;
using Quartz;
using JobSchedularDAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace JobSchedularWebServices.Services
{
    public class JobSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<JobSchedulerService> _logger;
        private const int MAX_RETRY = 3;

        public JobSchedulerService(IServiceScopeFactory scopeFactory, ILogger<JobSchedulerService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Job Scheduler Service started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var repo = scope.ServiceProvider.GetRequiredService<IJobSchedular>();

                       
                        var dueJobIds = await repo.GetDueJobIdsAsync(DateTime.UtcNow);

                        if (dueJobIds.Any())
                        {
                            var tasks = dueJobIds.Select(id => Task.Run(() => ProcessJobById(id), stoppingToken));
                            await Task.WhenAll(tasks);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in main scheduler heartbeat.");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private async Task ProcessJobById(string jobId)
        {
            
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IJobSchedular>();

                var job = await repo.ClaimJobAsync(jobId);
                if (job == null) return;

                try
                {
                    _logger.LogInformation($"Processing Job {jobId}...");
                    await ExecuteJobWithLoggingAsync(job, repo);

                    var nextRun = CalculateNextRun(job.SchedulePattern);
                    if (nextRun.HasValue)
                    {
                        job.ScheduledExecutionTime = nextRun.Value;
                        job.Status = "Scheduled";
                    }
                    else
                    {
                        job.Status = "Completed";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Job {jobId} failed. Handling retry...");
                    await HandleJobRetry(job.JobId, repo);
                    job.Status = "Scheduled";
                }
                finally
                {
                    await repo.UpdateJobSchedule(job);
                }
            }
        }

        private async Task ExecuteJobWithLoggingAsync(JobSchedule schedule, IJobSchedular repo)
        {
            var log = new JobExecutionLog
            {
                ExecutionLogId = Guid.NewGuid().ToString(),
                JobId = schedule.JobId,
                StartTime = DateTime.UtcNow,
                ExecutionStatus = "Running"
            };

            await repo.AddJobExecutionLog(log);

            try
            {
                var jobDef = await repo.GetJobDefinitionById(schedule.JobId);
                if (jobDef == null) throw new Exception("Job definition missing.");

                 
                await RunJobLogic(jobDef);

                log.EndTime = DateTime.UtcNow;
                log.ExecutionStatus = "Completed";
            }
            catch (Exception ex)
            {
                log.EndTime = DateTime.UtcNow;
                log.ExecutionStatus = "Failed";
                //log.ErrorMessage = ex.Message;
                throw;
            }
            finally
            {
                await repo.UpdateJobExecutionLog(log);
            }
        }

        private async Task RunJobLogic(JobDefinition job)
        {
            switch (job.JobName)
            {
                case "SendEmail":
                    // Call the Email Microservice API
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var httpClientFactory = scope.ServiceProvider.GetRequiredService<IHttpClientFactory>();
                        var client = httpClientFactory.CreateClient();

                        // Use the URL where your Email Microservice is running
                        var response = await client.PostAsync("https://email-service-url/api/email/trigger-pending-emails", null);

                        if (!response.IsSuccessStatusCode)
                        {
                            throw new Exception($"Email service returned {response.StatusCode}");
                        }
                    }
                    break;

                default:
                    _logger.LogWarning($"No logic defined for job type: {job.JobName}");
                    break;
            }
        }

        private async Task HandleJobRetry(string jobId, IJobSchedular repo)
        {
            var jobRetry = await repo.GetJobRetryById(jobId);
            if (jobRetry == null)
            {
                await repo.AddJobRetry(new JobRetry
                {
                    RetryId = Guid.NewGuid().ToString(),
                    JobId = jobId,
                    RetryAttemptNumber = 1,
                    RetryStatus = "Pending",
                    RetryTime = DateTime.UtcNow.AddMinutes(5)
                });
            }
            else if (jobRetry.RetryAttemptNumber < MAX_RETRY)
            {
                jobRetry.RetryAttemptNumber++;
                jobRetry.RetryTime = DateTime.UtcNow.AddMinutes(Math.Pow(2, jobRetry.RetryAttemptNumber)); // Exponential backoff
                await repo.UpdateJobRetry(jobRetry);
            }
            else
            {
                _logger.LogCritical($"Job {jobId} permanently failed after {MAX_RETRY} retries.");
            }
        }

        private DateTime? CalculateNextRun(string cronExpression)
        {
            if (string.IsNullOrEmpty(cronExpression) || !CronExpression.IsValidExpression(cronExpression))
                return null;

            var cron = new CronExpression(cronExpression);
            return cron.GetNextValidTimeAfter(DateTimeOffset.UtcNow)?.DateTime;
        }
    }
}