using JobSchedularDAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace JobSchedularDAL
{
    public class JobSchedularRepository : IJobSchedular
    {
        private readonly JobSchedularDbContext _context;
        private readonly ILogger<JobSchedularRepository> _logger;

        public JobSchedularRepository(ILogger<JobSchedularRepository> logger)
        {
            _context = new JobSchedularDbContext();
            _logger = logger;
        }

        #region User
        public async Task<bool> AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                await _context.Users.AddAsync(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                return await _context.Users.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return null; // Let caller know something went wrong
            }
        }

        public async Task<User> GetUserById(string userId)
        {
            try
            {
                return await _context.Users.FindAsync(userId);
            }
            catch (Exception ex)
            {
                // Log the exception properly
                Console.WriteLine($"Error fetching user by ID: {ex.Message}");
                return null;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {

            try
            {
                var user = await _context.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching user by ID: " + ex.Message);
                return null;
            }
        }

        public async Task<JobSchedule> ClaimJobAsync(string jobId)
        {
            try
            {
                var rowsAffected = await _context.JobSchedules
                .Where(s => s.JobId == jobId && s.Status == "Scheduled")
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(s => s.Status, "Running"));

                if (rowsAffected == 0)
                {
                    return null;
                }
                else
                {
                    return await _context.JobSchedules.FirstOrDefaultAsync(s => s.JobId == jobId);
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error claiming job with ID {JobId}", jobId);  
                return null;
            }
            
        }
        public async Task<bool> UpdateUser(User user)
        {
            bool status = true;
            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserId);
                if(existingUser == null)
                {
                    _logger.LogWarning($"User with ID {user.UserId} not found.");
                    return false;
                }
                else
                {
                    //existingUser.Username = user.Username;
                    //existingUser.Email = user.Email;
                    //existingUser.Password = user.Password;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user: " + ex.Message);
                status = false;
            }
            return status;

        }

        public async Task<bool> DeleteUser(string userId)
        {
            bool status = false;
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChangesAsync();
                    status = true;
                }
                else
                {
                    _logger.LogWarning($"User with ID {userId} not found.");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting user: " + ex.Message);
            }
            return status;
        }


        #endregion

        #region Job Definition
        public async Task<bool> AddJobDefinition(JobDefinition jobDefinition)
        {
            bool status = false;
            try
            {
                await _context.JobDefinitions.AddAsync(jobDefinition);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job definition: " + ex.Message);
            }
            return status;
        }

        public async Task<List<JobDefinition>> GetJobDefinitions()
        {

            try
            {
                var jobDefinitions = await _context.JobDefinitions.AsNoTracking().ToListAsync();
                return jobDefinitions;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definitions: " + ex.Message);
                return null;
            }
        }

        public async Task<JobDefinition> GetJobDefinitionById(string jobId)
        {
            JobDefinition jobDefinition = new JobDefinition();
            try
            {
                jobDefinition = await _context.JobDefinitions.AsNoTracking().Where(j => j.JobId == jobId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definition by ID: " + ex.Message);
            }
            return jobDefinition;
        }
        public async Task<List<JobDefinition>> GetJobDefinitionsByUserId(string userId)
        {
            List<JobDefinition> jobDefinitions = new List<JobDefinition>();
            try
            {
                jobDefinitions = _context.JobDefinitions.AsNoTracking().Where(jd => jd.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definitions by user ID: " + ex.Message);
            }
            return jobDefinitions;
        }

        public async Task<JobDefinition> GetJobDefinitionByDefinitionName(string jobName)
        {
            JobDefinition jobDefinition = new JobDefinition();
            try
            {
                jobDefinition = await _context.JobDefinitions.AsNoTracking().Where(jd => jd.JobName == jobName).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definition by name: " + ex.Message);
            }
            return jobDefinition;
        }

        public async Task<bool> UpdateJobDefinition(JobDefinition jobDefinition)
        {
            bool status = false;
            try
            {
                JobDefinition existingJobDefinition = await _context.JobDefinitions.FindAsync(jobDefinition.JobId);
                if (existingJobDefinition == null)
                {
                    _logger.LogWarning($"JobDefinition with ID {jobDefinition.JobId} not found.");
                    status = false;
                }
                else
                {
                    _context.JobDefinitions.Update(jobDefinition);
                    _context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job definition: " + ex.Message);
            }
            return status;
        }

        public async Task<bool> DeleteJobDefinition(string jobId)
        {
            bool status = false;
            try
            {
                var jobDefinition = await _context.JobDefinitions.FindAsync(jobId);
                if (jobDefinition != null)
                {
                    _context.JobDefinitions.Remove(jobDefinition);
                    _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting job definition: " + ex.Message);
            }
            return status;
        }
        #endregion

        #region Job Schedule
        public async Task<bool> AddJobSchedule(JobSchedule jobSchedule)
        {
            bool status = false;
            try
            {
                _context.JobSchedules.Add(jobSchedule);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job schedule: " + ex.Message);
            }
            return status;
        }

        public async Task<List<JobSchedule>> GetJobSchedules()
        {
            try
            {
                var jobSchedules = _context.JobSchedules.ToList();
                return jobSchedules;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job schedules: " + ex.Message);
                return null;
            }

        }
        public async Task<JobSchedule> GetJobScheduleByJobId(string jobId)
        {
            JobSchedule jobSchedule = new JobSchedule();
            try
            {
                jobSchedule = _context.JobSchedules.Where(js => js.JobId == jobId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job schedule by job ID: " + ex.Message);
            }
            return jobSchedule;
        }

        public async Task<bool> UpdateJobSchedule(JobSchedule jobSchedule)
        {
            bool status = false;
            try
            {
                _context.JobSchedules.Update(jobSchedule);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job schedule: " + ex.Message);
            }
            return status;
        }
        public async Task<bool> DeleteJobSchedule(string jobId)
        {
            bool status = false;
            try
            {
                var jobSchedule = _context.JobSchedules.Find(jobId);
                if (jobSchedule != null)
                {
                    _context.JobSchedules.Remove(jobSchedule);
                    _context.SaveChanges();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting job schedule: " + ex.Message);
            }
            return status;
        }
        #endregion  

        #region Job Execution Log 
        public async Task<bool> AddJobExecutionLog(JobExecutionLog jobExecutionLog)
        {
            bool status = false;
            try
            {
                await _context.JobExecutionLogs.AddAsync(jobExecutionLog);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job execution log: " + ex.Message);
            }
            return status;
        }

        public async Task<List<JobExecutionLog>> GetJobExecutionLogs()
        {
            try
            {
                var jobExecutionLogs = await _context.JobExecutionLogs.ToListAsync();
                return jobExecutionLogs;
            }
            catch (Exception)
            {
                Console.WriteLine("Error fetching job execution logs: ");
                return null;

            }
        }

        public async Task<JobExecutionLog> GetJobExecutionLogsByJobId(string jobId)
        {
            JobExecutionLog jobExecutionLogs = new JobExecutionLog();
            try
            {
                jobExecutionLogs = await _context.JobExecutionLogs.Where(jel => jel.JobId == jobId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job execution logs by job ID: " + ex.Message);
            }
            return jobExecutionLogs;
        }

        public async Task<JobExecutionLog> GetJobExecutionLogById(string executionLogId)
        {
            JobExecutionLog jobExecutionLog = new JobExecutionLog();
            try
            {
                jobExecutionLog = await _context.JobExecutionLogs.FindAsync(executionLogId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job execution log by ID: " + ex.Message);
            }
            return jobExecutionLog;
        }

        public async Task<List<JobExecutionLog>> GetExecutionLogByStatus(string status)
        {
            try
            {
                var jobExecutionLog = await _context.JobExecutionLogs.Where(jel => jel.ExecutionStatus == status).ToListAsync();
                return jobExecutionLog;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job execution log by status: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> UpdateJobExecutionLog(JobExecutionLog jobExecutionLog)
        {
            bool status = false;
            try
            {
                var existingLog = await _context.JobExecutionLogs.FindAsync(jobExecutionLog.ExecutionLogId);
                if (existingLog == null)
                {
                    _logger.LogWarning($"JobExecutionLog with ID {jobExecutionLog.ExecutionLogId} not found.");
                    status = false;
                }
                else
                {
                    _context.JobExecutionLogs.Update(jobExecutionLog);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job execution log: " + ex.Message);
            }
            return status;
        }

        public async Task<bool> DeleteJobExecutionLog(string executionLogId)
        {
            bool status = false;
            try
            {
                var jobExecutionLog = await _context.JobExecutionLogs.FindAsync(executionLogId);
                if (jobExecutionLog != null)
                {
                    _context.JobExecutionLogs.Remove(jobExecutionLog);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting job execution log: " + ex.Message);
                status = false;
                return status;
            }
        }


        #endregion

        #region Execution Node
        public async Task<bool> AddExecutionNode(ExecutionNode executionNode)
        {
            bool status = false;
            try
            {
                await _context.ExecutionNodes.AddAsync(executionNode);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding execution node: " + ex.Message);
            }
            return status;
        }

        public async Task<List<ExecutionNode>> GetExecutionNodes()
        {
            List<ExecutionNode> executionNodes = new List<ExecutionNode>();
            try
            {
                executionNodes = await _context.ExecutionNodes.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching execution nodes: " + ex.Message);
            }
            return executionNodes;
        }

        public async Task<ExecutionNode> GetExecutionNodeById(string nodeId)
        {
            ExecutionNode executionNode = new ExecutionNode();
            try
            {
                executionNode = await _context.ExecutionNodes.FindAsync(nodeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching execution node by ID: " + ex.Message);
            }
            return executionNode;
        }

        public async Task<ExecutionNode> GetExecutionNodeByIpAddress(string ipAddress)
        {
            ExecutionNode executionNode = new ExecutionNode();
            try
            {
                executionNode = await _context.ExecutionNodes.Where(en => en.NodeIpaddress == ipAddress).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching execution node by IP address: " + ex.Message);
            }
            return executionNode;
        }

        public async Task<string> CheckExecutionNodeStatus(string nodeId)
        {
            string status = string.Empty;
            try
            {
                var executionNode = await _context.ExecutionNodes.FindAsync(nodeId);
                if (executionNode != null)
                {
                    status = executionNode.NodeStatus;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error checking execution node status: " + ex.Message);
            }
            return status;
        }
        public async Task<bool> UpdateExecutionNode(ExecutionNode executionNode)
        {
            bool status = false;
            try
            {
                var existingNode = await _context.ExecutionNodes.FindAsync(executionNode.NodeId);
                if (existingNode != null)
                {
                    _context.ExecutionNodes.Update(executionNode);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                else
                {
                    _logger.LogWarning($"ExecutionNode with ID {executionNode.NodeId} not found.");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating execution node: " + ex.Message);
            }
            return status;
        }

        public async Task<bool> DeleteExecutionNode(string nodeId)
        {
            bool status = false;
            try
            {
                var executionNode = await _context.ExecutionNodes.FindAsync(nodeId);
                if (executionNode != null)
                {
                    _context.ExecutionNodes.Remove(executionNode);
                    await _context.SaveChangesAsync();
                    status = true;
                }
                else
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating execution node: " + ex.Message);
            }
            return status;
        }
        #endregion

        #region Resource Allocation
        public async Task<bool> AddResourceAllocation(ResourceAllocation resourceAllocation)
        {
            bool status = false;
            try
            {
                await _context.ResourceAllocations.AddAsync(resourceAllocation);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding resource allocation: " + ex.Message);
            }
            return status;
        }

        public async Task<List<ResourceAllocation>> GetResourceAllocations()
        {
            List<ResourceAllocation> resourceAllocations = new List<ResourceAllocation>();
            try
            {
                resourceAllocations = await _context.ResourceAllocations.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocations: " + ex.Message);
            }
            return resourceAllocations;
        }

        public async Task<List<string>> GetDueJobIdsAsync(DateTime now)
        {
            try
            {
                return await _context.JobSchedules
                .Where(s => s.Status == "Scheduled" && s.ScheduledExecutionTime <= now)
                .Select(s => s.JobId)
                .ToListAsync();
            }
            catch (Exception)
            {
                _logger.LogError("Error fetching due job IDs.");
                return new List<string>();

            }
            
        }
        public async Task<List<ResourceAllocation>> GetResourceAllocationsByJobId(string jobId)
        {
            List<ResourceAllocation> resourceAllocations = new List<ResourceAllocation>();
            try
            {
                resourceAllocations = await _context.ResourceAllocations.Where(ra => ra.JobId == jobId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocations by job ID: " + ex.Message);
            }
            return resourceAllocations;
        }

        public async Task<ResourceAllocation> GetResourceAllocationById(string allocationId)
        {
            ResourceAllocation resourceAllocation = new ResourceAllocation();
            try
            {
                resourceAllocation = await _context.ResourceAllocations.FindAsync(allocationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocation by ID: " + ex.Message);
            }
            return resourceAllocation;
        }

        public async Task<ResourceAllocation> GetResourceAllocationByexecutionNodeId(string executionNodeId)
        {
            ResourceAllocation resourceAllocation = new ResourceAllocation();
            try
            {
                resourceAllocation = await _context.ResourceAllocations.Where(ra => ra.ExecutionNodeId == executionNodeId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocation by execution node ID: " + ex.Message);
            }
            return resourceAllocation;
        }

        public async Task<bool> UpdateResourceAllocation(ResourceAllocation resourceAllocation)
        {
            bool status = false;
            try
            {
                _context.ResourceAllocations.Update(resourceAllocation);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating resource allocation: " + ex.Message);
            }
            return status;
        }

        public async Task<bool> DeleteResourceAllocation(string allocationId)
        {
            bool status = false;
            try
            {
                var resourceAllocation = await _context.ResourceAllocations.FindAsync(allocationId);
                if (resourceAllocation != null)
                {
                    _context.ResourceAllocations.Remove(resourceAllocation);
                    await _context.SaveChangesAsync();
                    status = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting resource allocation: " + ex.Message);
            }
            return status;
        }

        #endregion

        #region Job Retry
        public async Task<bool> AddJobRetry(JobRetry jobRetry)
        {
            bool status = false;
            try
            {
                await _context.JobRetries.AddAsync(jobRetry);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job retry: " + ex.Message);
            }
            return status;
        }

        public async Task<List<JobRetry>> GetJobRetries()
        {
            List<JobRetry> jobRetries = new List<JobRetry>();
            try
            {
                jobRetries = await _context.JobRetries.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retries: " + ex.Message);
            }
            return jobRetries;
        }

        public async Task<JobRetry> GetJobRetryById(string retryId)
        {
            JobRetry jobRetry = new JobRetry();
            try
            {
                jobRetry = await _context.JobRetries.FindAsync(retryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retry by ID: " + ex.Message);
            }
            return jobRetry;
        }

        public async Task<int> GetJobRetryCount(string jobId)
        {
            int cnt = 0;
            try
            {
                var jobRetry = await _context.JobRetries.Where(jb => jb.JobId == jobId).FirstOrDefaultAsync();
                if (jobRetry != null)
                {
                    cnt = jobRetry.RetryAttemptNumber;
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retryAttemptNumber", ex.Message);
            }

            return cnt;

        }

        public async Task<string> JobRetryStatus(string jobId)
        {
            string status = string.Empty;
            try
            {
                var jobRetry = await _context.JobRetries.Where(jb => jb.JobId == jobId).FirstOrDefaultAsync();
                if (jobRetry != null)
                {
                    status = jobRetry.RetryStatus;
                }
                else
                {
                    status = "";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retry status", ex.Message);
            }

            return status;
        }

        public async Task<bool> UpdateJobRetry(JobRetry jobRetry)
        {
            bool status = false;
            try
            {
                var existingJobRetry = await _context.JobRetries.Where(jr => jr.RetryId == jobRetry.RetryId).FirstOrDefaultAsync();
                _context.JobRetries.Update(jobRetry);
                await _context.SaveChangesAsync();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job retry: " + ex.Message);
            }
            return status;
        }

        public async Task<bool> DeleteJobRetry(string jobId)
        {
            bool status = false;
            try
            {
                var jobRetry = await _context.JobRetries.Where(jr => jr.JobId == jobId).FirstOrDefaultAsync();
                if (jobRetry != null)
                {
                    _context.JobRetries.Remove(jobRetry);
                    _context.SaveChanges();
                    status = true;
                }
                else
                {
                    _logger.LogWarning($"JobRetry with Job ID {jobId} not found.");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting job retry: " + ex.Message);
            }
            return status;
        }
        #endregion
    }
}
