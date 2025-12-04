using JobSchedularDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace JobSchedularDAL
{
    public class JobSchedularRepository : IJobSchedular
    {
        public readonly JobSchedularDbContext _context;

        public JobSchedularRepository()
        {
            _context = new JobSchedularDbContext();
        }

        #region User
        public bool AddUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        public List<User>? GetUsers()
        {
            try
            {
                return _context.Users.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                return null; // Let caller know something went wrong
            }
        }

        public User? GetUserById(string userId)
        {
            try
            {
                return _context.Users.Find(userId);
            }
            catch (Exception ex)
            {
                // Log the exception properly
                Console.WriteLine($"Error fetching user by ID: {ex.Message}");
                return null;
            }
        }

        public User GetUserByEmail(string email)
        {

            try
            {
                var user = _context.Users.Where(user => user.Email == email).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching user by ID: " + ex.Message);
                return null;
            }
        }

        public bool UpdateUser(User user)
        {
            bool status = true;
            try
            {
                _context.Users.Update(user);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user: " + ex.Message);
                status = false;
            }
            return status;

        }

        public bool DeleteUser(string userId)
        {
            bool status = false;
            try
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    status = true;
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
        public bool AddJobDefinition(JobDefinition jobDefinition)
        {
            bool status = false;
            try
            {
                _context.JobDefinitions.Add(jobDefinition);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job definition: " + ex.Message);
            }
            return status;
        }

        public List<JobDefinition> GetJobDefinitions()
        {

            try
            {
                var jobDefinitions = _context.JobDefinitions.ToList();
                return jobDefinitions;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definitions: " + ex.Message);
                return null;
            }
        }

        public JobDefinition GetJobDefinitionById(string jobId)
        {
            JobDefinition jobDefinition = new JobDefinition();
            try
            {
                jobDefinition = _context.JobDefinitions.Find(jobId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definition by ID: " + ex.Message);
            }
            return jobDefinition;
        }
        public List<JobDefinition> GetJobDefinitionsByUserId(string userId)
        {
            List<JobDefinition> jobDefinitions = new List<JobDefinition>();
            try
            {
                jobDefinitions = _context.JobDefinitions.Where(jd => jd.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definitions by user ID: " + ex.Message);
            }
            return jobDefinitions;
        }

        public JobDefinition GetJobDefinitionByDefinitionName(string jobName)
        {
            JobDefinition jobDefinition = new JobDefinition();
            try
            {
                jobDefinition = _context.JobDefinitions.Where(jd => jd.JobName == jobName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job definition by name: " + ex.Message);
            }
            return jobDefinition;
        }

        public bool UpdateJobDefinition(JobDefinition jobDefinition)
        {
            bool status = false;
            try
            {
                _context.JobDefinitions.Update(jobDefinition);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job definition: " + ex.Message);
            }
            return status;
        }

        public bool DeleteJobDefinition(string jobId)
        {
            bool status = false;
            try
            {
                var jobDefinition = _context.JobDefinitions.Find(jobId);
                if (jobDefinition != null)
                {
                    _context.JobDefinitions.Remove(jobDefinition);
                    _context.SaveChanges();
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
        public bool AddJobSchedule(JobSchedule jobSchedule)
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

        public List<JobSchedule> GetJobSchedules()
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
        public JobSchedule GetJobScheduleByJobId(string jobId)
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

        public bool UpdateJobSchedule(JobSchedule jobSchedule)
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
        public bool DeleteJobSchedule(string jobId)
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
        public bool AddJobExecutionLog(JobExecutionLog jobExecutionLog)
        {
            bool status = false;
            try
            {
                _context.JobExecutionLogs.Add(jobExecutionLog);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job execution log: " + ex.Message);
            }
            return status;
        }

        public List<JobExecutionLog> GetJobExecutionLogs()
        {
            try
            {
                var jobExecutionLogs = _context.JobExecutionLogs.ToList();
                return jobExecutionLogs;
            }
            catch (Exception)
            {
                Console.WriteLine("Error fetching job execution logs: ");
                return null;

            }
        }

        public JobExecutionLog GetJobExecutionLogsByJobId(string jobId)
        {
            JobExecutionLog jobExecutionLogs = new JobExecutionLog();
            try
            {
                jobExecutionLogs = _context.JobExecutionLogs.Where(jel => jel.JobId == jobId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job execution logs by job ID: " + ex.Message);
            }
            return jobExecutionLogs;
        }

        public JobExecutionLog GetJobExecutionLogById(string executionLogId)
        {
            JobExecutionLog jobExecutionLog = new JobExecutionLog();
            try
            {
                jobExecutionLog = _context.JobExecutionLogs.Find(executionLogId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job execution log by ID: " + ex.Message);
            }
            return jobExecutionLog;
        }

        public List<JobExecutionLog> GetExecutionLogByStatus(string status)
        {
            try
            {
                var jobExecutionLog = _context.JobExecutionLogs.Where(jel => jel.ExecutionStatus == status).ToList();
                return jobExecutionLog;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job execution log by status: " + ex.Message);
                return null;
            }
        }

        public bool UpdateJobExecutionLog(JobExecutionLog jobExecutionLog)
        {
            bool status = false;
            try
            {
                _context.JobExecutionLogs.Update(jobExecutionLog);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job execution log: " + ex.Message);
            }
            return status;
        }

        public bool DeleteJobExecutionLog(string executionLogId)
        {
            bool status = false;
            try
            {
                var jobExecutionLog = _context.JobExecutionLogs.Find(executionLogId);
                if (jobExecutionLog != null)
                {
                    _context.JobExecutionLogs.Remove(jobExecutionLog);
                    _context.SaveChanges();
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
        public bool AddExecutionNode(ExecutionNode executionNode)
        {
            bool status = false;
            try
            {
                _context.ExecutionNodes.Add(executionNode);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding execution node: " + ex.Message);
            }
            return status;
        }

        public List<ExecutionNode> GetExecutionNodes()
        {
            List<ExecutionNode> executionNodes = new List<ExecutionNode>();
            try
            {
                executionNodes = _context.ExecutionNodes.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching execution nodes: " + ex.Message);
            }
            return executionNodes;
        }

        public ExecutionNode GetExecutionNodeById(string nodeId)
        {
            ExecutionNode executionNode = new ExecutionNode();
            try
            {
                executionNode = _context.ExecutionNodes.Find(nodeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching execution node by ID: " + ex.Message);
            }
            return executionNode;
        }

        public ExecutionNode GetExecutionNodeByIpAddress(string ipAddress)
        {
            ExecutionNode executionNode = new ExecutionNode();
            try
            {
                executionNode = _context.ExecutionNodes.Where(en => en.NodeIpaddress == ipAddress).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching execution node by IP address: " + ex.Message);
            }
            return executionNode;
        }

        public string CheckExecutionNodeStatus(string nodeId)
        {
            string status = string.Empty;
            try
            {
                var executionNode = _context.ExecutionNodes.Find(nodeId);
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
        public bool UpdateExecutionNode(ExecutionNode executionNode)
        {
            bool status = false;
            try
            {
                _context.ExecutionNodes.Update(executionNode);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating execution node: " + ex.Message);
            }
            return status;
        }

        public bool DeleteExecutionNode(string nodeId)
        {
            bool status = false;
            try
            {
                var executionNode = _context.ExecutionNodes.Find(nodeId);
                if (executionNode != null)
                {
                    _context.ExecutionNodes.Remove(executionNode);
                    _context.SaveChanges();
                    status = true;
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
        public bool AddResourceAllocation(ResourceAllocation resourceAllocation)
        {
            bool status = false;
            try
            {
                _context.ResourceAllocations.Add(resourceAllocation);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding resource allocation: " + ex.Message);
            }
            return status;
        }

        public List<ResourceAllocation> GetResourceAllocations()
        {
            List<ResourceAllocation> resourceAllocations = new List<ResourceAllocation>();
            try
            {
                resourceAllocations = _context.ResourceAllocations.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocations: " + ex.Message);
            }
            return resourceAllocations;
        }

        public List<ResourceAllocation> GetResourceAllocationsByJobId(string jobId)
        {
            List<ResourceAllocation> resourceAllocations = new List<ResourceAllocation>();
            try
            {
                resourceAllocations = _context.ResourceAllocations.Where(ra => ra.JobId == jobId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocations by job ID: " + ex.Message);
            }
            return resourceAllocations;
        }

        public ResourceAllocation GetResourceAllocationById(string allocationId)
        {
            ResourceAllocation resourceAllocation = new ResourceAllocation();
            try
            {
                resourceAllocation = _context.ResourceAllocations.Find(allocationId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocation by ID: " + ex.Message);
            }
            return resourceAllocation;
        }

        public ResourceAllocation GetResourceAllocationByexecutionNodeId(string executionNodeId)
        {
            ResourceAllocation resourceAllocation = new ResourceAllocation();
            try
            {
                resourceAllocation = _context.ResourceAllocations.Where(ra => ra.ExecutionNodeId == executionNodeId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching resource allocation by execution node ID: " + ex.Message);
            }
            return resourceAllocation;
        }

        public bool UpdateResourceAllocation(ResourceAllocation resourceAllocation)
        {
            bool status = false;
            try
            {
                _context.ResourceAllocations.Update(resourceAllocation);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating resource allocation: " + ex.Message);
            }
            return status;
        }

        public bool DeleteResourceAllocation(string allocationId)
        {
            bool status = false;
            try
            {
                var resourceAllocation = _context.ResourceAllocations.Find(allocationId);
                if (resourceAllocation != null)
                {
                    _context.ResourceAllocations.Remove(resourceAllocation);
                    _context.SaveChanges();
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
        public bool AddJobRetry(JobRetry jobRetry)
        {
            bool status = false;
            try
            {
                _context.JobRetries.Add(jobRetry);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding job retry: " + ex.Message);
            }
            return status;
        }

        public List<JobRetry> GetJobRetries()
        {
            List<JobRetry> jobRetries = new List<JobRetry>();
            try
            {
                jobRetries = _context.JobRetries.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retries: " + ex.Message);
            }
            return jobRetries;
        }

        public JobRetry GetJobRetryById(string retryId)
        {
            JobRetry jobRetry = new JobRetry();
            try
            {
                jobRetry = _context.JobRetries.Find(retryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retry by ID: " + ex.Message);
            }
            return jobRetry;
        }

        public int GetJobRetryCount(string jobId)
        {
            int cnt = 0;
            try
            {
                var jobRetry = _context.JobRetries.Where(jb => jb.JobId == jobId).FirstOrDefault();
                cnt = jobRetry.RetryAttemptNumber;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retryAttemptNumber", ex.Message);
            }

            return cnt;

        }

        public string JobRetryStatus(string jobId)
        {
            string status = string.Empty;
            try
            {
                var jobRetry = _context.JobRetries.Where(jb => jb.JobId == jobId).FirstOrDefault();
                status = jobRetry.RetryStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching job retry status", ex.Message);
            }

            return status;
        }

        public bool UpdateJobRetry(JobRetry jobRetry)
        {
            bool status = false;
            try
            {
                _context.JobRetries.Update(jobRetry);
                _context.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating job retry: " + ex.Message);
            }
            return status;
        }

        public bool DeleteJobRetry(string jobId)
        {
            bool status = false;
            try
            {
                var jobRetry = _context.JobRetries.Where(jr => jr.JobId == jobId).FirstOrDefault();
                if (jobRetry != null)
                {
                    _context.JobRetries.Remove(jobRetry);
                    _context.SaveChanges();
                    status = true;
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
