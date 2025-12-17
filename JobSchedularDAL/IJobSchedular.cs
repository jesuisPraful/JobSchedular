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
        Task<bool> AddUser(User user);
        Task<List<User>> GetUsers();
        Task<User> GetUserById(string userId);
        Task<User> GetUserByEmail(string email);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(string userId);
        #endregion

        #region Job Definition
        Task<bool> AddJobDefinition(JobDefinition jobDefinition);
        Task<List<JobDefinition>> GetJobDefinitions();
        Task<JobDefinition> GetJobDefinitionById(string jobId);
        Task<List<JobDefinition>> GetJobDefinitionsByUserId(string userId);
        Task<JobDefinition> GetJobDefinitionByDefinitionName(string jobName);
        Task<bool> UpdateJobDefinition(JobDefinition jobDefinition);
        Task<bool> DeleteJobDefinition(string jobId);
        #endregion

        #region Job Schedule
        Task<bool> AddJobSchedule(JobSchedule jobSchedule);
        Task<List<JobSchedule>> GetJobSchedules();
        Task<List<string>> GetDueJobIdsAsync(DateTime now);
        Task<JobSchedule> ClaimJobAsync(string jobId);
        Task<JobSchedule> GetJobScheduleByJobId(string jobId);
        Task<bool> UpdateJobSchedule(JobSchedule jobSchedule);
        Task<bool> DeleteJobSchedule(string jobId);
        #endregion

        #region Job Execution Log
        Task<bool> AddJobExecutionLog(JobExecutionLog jobExecutionLog);
        Task<List<JobExecutionLog>> GetJobExecutionLogs();
        Task<JobExecutionLog> GetJobExecutionLogsByJobId(string jobId);
        Task<JobExecutionLog> GetJobExecutionLogById(string executionLogId);
        Task<List<JobExecutionLog>> GetExecutionLogByStatus(string status);
        Task<bool> UpdateJobExecutionLog(JobExecutionLog jobExecutionLog);
        Task<bool> DeleteJobExecutionLog(string executionLogId);
        #endregion

        #region Execution Node
        Task<bool> AddExecutionNode(ExecutionNode executionNode);
        Task<List<ExecutionNode>> GetExecutionNodes();
        Task<ExecutionNode> GetExecutionNodeById(string nodeId);
        Task<ExecutionNode> GetExecutionNodeByIpAddress(string ipAddress);
        Task<string> CheckExecutionNodeStatus(string nodeId);
        Task<bool> UpdateExecutionNode(ExecutionNode executionNode);
        Task<bool> DeleteExecutionNode(string nodeId);
        #endregion

        #region Resource Allocation
        Task<bool> AddResourceAllocation(ResourceAllocation resourceAllocation);
        Task<List<ResourceAllocation>> GetResourceAllocations();
        Task<List<ResourceAllocation>> GetResourceAllocationsByJobId(string jobId);
        Task<ResourceAllocation> GetResourceAllocationById(string allocationId);
        Task<ResourceAllocation> GetResourceAllocationByexecutionNodeId(string executionNodeId);
        Task<bool> UpdateResourceAllocation(ResourceAllocation resourceAllocation);
        Task<bool> DeleteResourceAllocation(string allocationId);
        #endregion

        #region Job Retry
        Task<bool> AddJobRetry(JobRetry jobRetry);
        Task<List<JobRetry>> GetJobRetries();
        Task<JobRetry> GetJobRetryById(string retryId);
        Task<int> GetJobRetryCount(string jobId);
        Task<string> JobRetryStatus(string jobId);
        Task<bool> UpdateJobRetry(JobRetry jobRetry);
        Task<bool> DeleteJobRetry(string jobId);
        #endregion
    }
}
