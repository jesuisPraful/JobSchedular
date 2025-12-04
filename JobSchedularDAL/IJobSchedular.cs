using JobSchedularDAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobSchedularDAL
{
    public interface IJobSchedular
    {
        #region User
        public bool AddUser(User user);
        public List<User> GetUsers();
        public User? GetUserById(string userId);
        public bool UpdateUser(User user);
        public User GetUserByEmail(string email);
        public bool DeleteUser(string userId);
        #endregion

        #region Job Definition
        public bool AddJobDefinition(JobDefinition jobDefinition);
        public List<JobDefinition> GetJobDefinitions();
        public List<JobDefinition> GetJobDefinitionsByUserId(string userId);
        public JobDefinition GetJobDefinitionById(string jobId);

        public JobDefinition GetJobDefinitionByDefinitionName(string jobName);

        public bool UpdateJobDefinition(JobDefinition jobDefinition);

        public bool DeleteJobDefinition(string jobId);
        #endregion

        #region Job Schedule
        public bool AddJobSchedule(JobSchedule jobSchedule);
        public JobSchedule GetJobScheduleByJobId(string jobId);
        public bool UpdateJobSchedule(JobSchedule jobSchedule);
        public bool DeleteJobSchedule(string jobId);
        public List<JobSchedule> GetJobSchedules();
        #endregion

        #region Job Execution Log 
        public bool DeleteJobExecutionLog(string executionLogId);
        public bool UpdateJobExecutionLog(JobExecutionLog jobExecutionLog);
        public List<JobExecutionLog> GetExecutionLogByStatus(string status);
        public JobExecutionLog GetJobExecutionLogById(string executionLogId);
        public bool AddJobExecutionLog(JobExecutionLog jobExecutionLog);
        public JobExecutionLog GetJobExecutionLogsByJobId(string jobId);
        public List<JobExecutionLog> GetJobExecutionLogs();
        #endregion

        #region Execution Node
        public bool AddExecutionNode(ExecutionNode executionNode);
        public List<ExecutionNode> GetExecutionNodes();

        public ExecutionNode GetExecutionNodeById(string nodeId);
        public ExecutionNode GetExecutionNodeByIpAddress(string ipAddress);
        public string CheckExecutionNodeStatus(string nodeId);
        public bool UpdateExecutionNode(ExecutionNode executionNode);
        public bool DeleteExecutionNode(string nodeId);
        #endregion

        #region Resource Allocation
        public bool AddResourceAllocation(ResourceAllocation resourceAllocation);
        public List<ResourceAllocation> GetResourceAllocationsByJobId(string jobId);
        public ResourceAllocation GetResourceAllocationById(string allocationId);
        public ResourceAllocation GetResourceAllocationByexecutionNodeId(string executionNodeId);
        public bool UpdateResourceAllocation(ResourceAllocation resourceAllocation);
        public bool DeleteResourceAllocation(string allocationId);
        public List<ResourceAllocation> GetResourceAllocations();


        #endregion

        #region Job Retry
        public bool AddJobRetry(JobRetry jobRetry);
        public List<JobRetry> GetJobRetries();
        public JobRetry GetJobRetryById(string retryId);
        public int GetJobRetryCount(string jobId);
        public string JobRetryStatus(string jobId);
        public bool UpdateJobRetry(JobRetry jobRetry);
        public bool DeleteJobRetry(string jobId);
        #endregion
    }
}
