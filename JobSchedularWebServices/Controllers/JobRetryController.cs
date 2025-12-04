using JobSchedularDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRetryController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public JobRetryController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddJobRetry(Models.JobRetry jobRetry)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var jobRetryEntity = new JobSchedularDAL.Models.JobRetry
                    {
                        RetryId = jobRetry.RetryId,
                        JobId = jobRetry.JobId,
                        RetryAttemptNumber = jobRetry.RetryAttemptNumber,
                        RetryStatus = jobRetry.RetryStatus,
                        RetryTime = jobRetry.RetryTime
                    };

                    var status = _repository.AddJobRetry(jobRetryEntity);
                    if (status)
                    {
                        return Ok("Job Retry Added Successfully");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error adding job retry.");
                    }
                }
                else
                {
                    return BadRequest("Invalid data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding job retry: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetJobRetries()
        {
            try
            {
                var jobRetries = _repository.GetJobRetries();
                if (jobRetries == null || jobRetries.Count == 0)
                {
                    return NotFound("No job retries found.");
                }
                return Ok(jobRetries);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving job retries: " + ex.Message);
            }
        }

        [HttpGet("retryId/{retryId}")]
        public IActionResult GetJobRetryById(string retryId)
        {
            if(string.IsNullOrEmpty(retryId))  return BadRequest("Retry ID is required.");
            
            try
            {
                var jobRetry = _repository.GetJobRetryById(retryId);
                if (jobRetry == null)
                {
                    return NotFound("Job retry not found.");
                }
                return Ok(jobRetry);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving job retry: " + ex.Message);
            }
        }


        [HttpGet("count/{jobId}")]
        public IActionResult GetJobRetryCount(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) return BadRequest("Job ID is required.");

            try
            {
                var retryCount = _repository.GetJobRetryCount(jobId);
                return Ok(retryCount);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving job retry count: " + ex.Message);
            }
        }

        [HttpGet("status/{jobId}")]
        public IActionResult GetJobRetryStatus(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) return BadRequest("Job ID is required.");

            try
            {
                var retryStatus = _repository.JobRetryStatus(jobId);
                if (retryStatus.IsNullOrEmpty())
                {
                    return NotFound("No retry status found for the given Job ID.");
                }
                return Ok(retryStatus);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving job retry status: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateJobRetry(Models.JobRetry jobRetry)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jobRetryEntity = new JobSchedularDAL.Models.JobRetry
                    {
                        RetryId = jobRetry.RetryId,
                        JobId = jobRetry.JobId,
                        RetryAttemptNumber = jobRetry.RetryAttemptNumber,
                        RetryStatus = jobRetry.RetryStatus,
                        RetryTime = jobRetry.RetryTime
                    };

                    var status = _repository.UpdateJobRetry(jobRetryEntity);
                    if (status)
                    {
                        return Ok("Job Retry Updated Successfully");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, "Error updating job retry.");
                    }
                }
                else
                {
                    return BadRequest("Invalid data.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating job retry.");
            }
        }

        [HttpDelete("jobId/{jobId}")]
        public IActionResult DeleteJobRetry(string jobId)
        {
            if (string.IsNullOrEmpty(jobId)) return BadRequest("Job ID is required.");

            try
            {
                var status = _repository.DeleteJobRetry(jobId);
                if (status)
                {
                    return Ok("Job Retry Deleted Successfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting job retry.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting job retry: " + ex.Message);
            }
        }

    }
}
