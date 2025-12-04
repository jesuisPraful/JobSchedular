using JobSchedularDAL;
using JobSchedularDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobExecutionLogController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public JobExecutionLogController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddJobExecutionLog([FromBody] Models.JobExecutionLog jobExecutionLog)
        {
            if (jobExecutionLog == null)
            {
                return BadRequest("JobExecutionLog is null.");
            }

            try
            {
                JobExecutionLog jobExecutionLog1 = new JobExecutionLog
                {
                    ExecutionLogId = jobExecutionLog.ExecutionLogId,
                    JobId = jobExecutionLog.JobId,
                    ExecutionStatus = jobExecutionLog.ExecutionStatus,
                    StartTime = jobExecutionLog.StartTime,
                    EndTime = jobExecutionLog.EndTime,
                    ExecutionNodeId = jobExecutionLog.ExecutionNodeId
                };

                var result = _repository.AddJobExecutionLog(jobExecutionLog1);
                if (result)
                {
                    return Ok("JobExecutionLog added successfully.");
                }
                else
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet]
        public IActionResult GetJobEecutionLogs()
        {
            try
            {
                var jobExecutions = _repository.GetJobExecutionLogs();
                if (jobExecutions == null || jobExecutions.Count == 0)
                {
                    return NotFound("No Job Execution Logs Found");
                }
                return Ok(jobExecutions);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("jobId/{jobId}")]
        public IActionResult GetJobExecutionLogsByJobId(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return BadRequest("Invalid Job ID");
            }

            try
            {
                var jobExecutionLog = _repository.GetJobExecutionLogsByJobId(jobId);
                if (jobExecutionLog == null)
                {
                    return NotFound("Job Execution Log not found for the given Job ID");
                }
                return Ok(jobExecutionLog);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("status/{status}")]
        public IActionResult GetExecutionLogByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return BadRequest("Invalid Status");
            }

            try
            {
                var jobExecutionLogs = _repository.GetExecutionLogByStatus(status);
                if (jobExecutionLogs == null || jobExecutionLogs.Count == 0)
                {
                    return NotFound("No Job Execution Logs found for the given status");
                }
                return Ok(jobExecutionLogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("executionLogId/{executionLogId}")]
        public IActionResult GetJobExecutionLogById(string executionLogId)
        {
            if (string.IsNullOrWhiteSpace(executionLogId))
            {
                return BadRequest("Invalid Execution Log ID");
            }

            try
            {
                var jobExecutionLog = _repository.GetJobExecutionLogById(executionLogId);
                if (jobExecutionLog == null)
                {
                    return NotFound("Job Execution Log not found for the given Execution Log ID");
                }
                return Ok(jobExecutionLog);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut]
        public IActionResult UpdateJobExecutionLog(Models.JobExecutionLog jobExecutionLog)
        {
            var status = false;
            try
            {
                if (ModelState.IsValid)
                {
                    JobExecutionLog jobExecutionLog1 = new JobExecutionLog
                    {
                        ExecutionLogId = jobExecutionLog.ExecutionLogId,
                        JobId = jobExecutionLog.JobId,
                        ExecutionStatus = jobExecutionLog.ExecutionStatus,
                        StartTime = jobExecutionLog.StartTime,
                        EndTime = jobExecutionLog.EndTime,
                        ExecutionNodeId = jobExecutionLog.ExecutionNodeId
                    };

                    status = _repository.UpdateJobExecutionLog(jobExecutionLog1);
                    if (status)
                    {
                        return Ok("Job Execution Log updated successfully.");
                    }
                    else
                    {
                        return StatusCode(500, "A problem happened while handling your request.");
                    }
                }
                else
                {
                    return BadRequest("Invalid Data."); 
                }
            }
            catch (Exception)
            {

                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("executionLogId/{executionLogId}")]
        public IActionResult DeleteJobExecutionLog(string executionLogId)
        {
            if (string.IsNullOrWhiteSpace(executionLogId))
            {
                return BadRequest("Invalid Execution Log ID");
            }
            var status = false;
            try
            {
                status = _repository.DeleteJobExecutionLog(executionLogId);
                if (status)
                {
                    return Ok("Job Execution Log deleted successfully.");
                }
                else
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

    }
}
