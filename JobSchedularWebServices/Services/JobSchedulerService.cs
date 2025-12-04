using JobSchedularDAL;
using Quartz;
using JobSchedularDAL.Models;
namespace JobSchedularWebServices.Services
{
    // Example: Background Service that checks schedules
    public class JobSchedulerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<JobSchedulerService> _logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IJobSchedular>();

                    // 1. Get all ACTIVE job schedules
                    var schedules = repo.GetJobSchedules();

                    // 2. Check which jobs are due NOW
                    var dueJobs = schedules.Where(s =>
                         s.ScheduledExecutionTime <= DateTime.Now &&
                          s.Status == "Scheduled"
                    ).ToList();

                    // 3. Execute each due job
                    foreach (var job in dueJobs)
                    {
                        Models.JobSchedule job1 = new Models.JobSchedule
                        {
                            JobId = job.JobId,
                            ScheduledExecutionTime = job.ScheduledExecutionTime,
                            Status = job.Status
                        };
                        await ExecuteJobAsync(job1, repo);

                        // 4. Update next run time
                        job1.NextRunTime = CalculateNextRun(job1.SchedulePattern);
                        repo.UpdateJobSchedule(job);
                    }
                }

                // 5. Wait before checking again
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private DateTime? CalculateNextRun(string cronExpression)
        {
            try
            {
                // Validate cron
                if (!CronExpression.IsValidExpression(cronExpression))
                    return null;

                var cron = new CronExpression(cronExpression);

                // Calculate next run from NOW
                var nextFireTime = cron.GetNextValidTimeAfter(DateTimeOffset.Now);

                return nextFireTime?.DateTime;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cron calculation failed: {ex.Message}");
                return null;
            }
        }

        private async Task ExecuteJobAsync(Models.JobSchedule schedule, IJobSchedular repo)
        {
            // Create execution log
            JobExecutionLog log = new JobExecutionLog
            {
                JobId = schedule.JobId,
                StartTime = DateTime.Now,
                ExecutionStatus = "Running"
            };

            repo.AddJobExecutionLog(log);

            try
            {
                // Get job definition
                var jobDef = repo.GetJobDefinitionById(schedule.JobId);

                // Execute the actual job logic
                await RunJobLogic(jobDef);

                // Update log as successful
                log.EndTime = DateTime.Now;
                log.ExecutionStatus = "Completed";
                repo.UpdateJobExecutionLog(log);
            }
            catch (Exception ex)
            {
                // Update log as failed
                log.EndTime = DateTime.Now;
                log.ExecutionStatus = "Failed";
                //log.ErrorMessage = ex.Message;
                repo.UpdateJobExecutionLog(log);

                // Handle retry logic
                HandleJobRetry(schedule.JobId, repo);
            }
        }

        private async Task RunJobLogic(JobDefinition job)
        {
            //switch (job.JobName)
            //{
            //    case "SendEmail":
            //        await ExecuteEmailJob(job.JobParameters);
            //        break;

            //    //case "GenerateReport":
            //    //    await ExecuteReportJob(job.JobParameters);
            //    //    break;

            //    //case "CallApi":
            //    //    await ExecuteApiCall(job.JobParameters);
            //    //    break;

            //    default:
            //        throw new Exception("Unknown job type: " + job.JobName);
            //}
        }

        private async Task HandleJobRetry(string jobId, IJobSchedular repo)
        {
            try
            {
                var jobRetry =  repo.GetJobRetryById(jobId);

                if (jobRetry == null)
                {
                    // Create a new retry if none exists
                    jobRetry = new JobRetry
                    {
                        RetryId = Guid.NewGuid().ToString(),
                        JobId = jobId,
                        RetryAttemptNumber = 1,
                        RetryStatus = "Pending",
                        RetryTime = DateTime.Now.AddMinutes(5) // Example: retry after 5 minutes
                    };
                     repo.AddJobRetry(jobRetry);
                    Console.WriteLine($"Created first retry for JobId: {jobId}");
                }
                else
                {
                    // Increment retry attempt
                    jobRetry.RetryAttemptNumber += 1;
                    jobRetry.RetryStatus = "Pending";
                    jobRetry.RetryTime = DateTime.Now.AddMinutes(5); // Retry after 5 minutes

                    repo.UpdateJobRetry(jobRetry);
                    Console.WriteLine($"Updated retry attempt {jobRetry.RetryAttemptNumber} for JobId: {jobId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling job retry for JobId {jobId}: {ex.Message}");
            }
        }

    }
}
