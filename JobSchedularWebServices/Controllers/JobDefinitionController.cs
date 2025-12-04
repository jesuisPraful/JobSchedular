using JobSchedularDAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobDefinitionController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public JobDefinitionController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddJobDefinition(Models.JobDefinition jobDefinition)
        {
            bool status = false;
            try
            {
                if (ModelState.IsValid)
                {
                    JobSchedularDAL.Models.JobDefinition newJobDefinition = new JobSchedularDAL.Models.JobDefinition
                    {
                        JobId = Guid.NewGuid().ToString(),
                        //JobId = jobDefinition.JobId,
                        UserId = jobDefinition.UserId,
                        JobName = jobDefinition.JobName,
                        JobDescription = jobDefinition.JobDescription,
                        JobParameters = jobDefinition.JobParameters,
                        Status = jobDefinition.Status,
                        Timestamps = jobDefinition.Timestamps
                    };

                    status = _repository.AddJobDefinition(newJobDefinition);
                    if (status)
                        return Ok("Job Definition added successfully");
                    else
                        return BadRequest("Failed to Add Job Definition");
                }
                else
                {
                    return BadRequest("Invalid Data");
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet]
        public IActionResult GetJobDefinitions()
        {
            try
            {
                var jobDefinitionsList = _repository.GetJobDefinitions();

                if (jobDefinitionsList == null )
                    return NotFound("No Job Definitions found");

                return Ok(jobDefinitionsList);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("userId/{userId}")]
        public IActionResult GetJobDefinitionsByUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }
            try
            {
                var jobDefinitionsList = _repository.GetJobDefinitionsByUserId(userId);

                if (jobDefinitionsList == null)
                    return NotFound("No Job Definitions found for the given User ID");

                return Ok(jobDefinitionsList);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("jobId/{jobId}")]
        public IActionResult GetJobDefinitionById(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return BadRequest("Job ID cannot be null or empty.");
            }

            try
            {
                var jobDefinition = _repository.GetJobDefinitionById(jobId);

                if (jobDefinition == null)
                {
                    return NotFound($"Job Definition with ID '{jobId}' not found.");
                }

                return Ok(jobDefinition);
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("jobName/{jobName}")]
        public IActionResult GetJobDefinitionByDefinitionName(string jobName)
        {
            if (string.IsNullOrWhiteSpace(jobName))
            {
                return BadRequest("Job Name cannot be null or empty.");
            }

            try
            {
                var jobDefinition = _repository.GetJobDefinitionByDefinitionName(jobName);

                if (jobDefinition == null)
                {
                    return NotFound($"Job Definition with Name '{jobName}' not found.");
                }

                return Ok(jobDefinition);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpPut]
        public IActionResult UpdateJobDefinition(Models.JobDefinition jobDefinition)
        {
            bool status = false;
            try
            {
                if (ModelState.IsValid)
                {
                    JobSchedularDAL.Models.JobDefinition updatedJobDefinition = new JobSchedularDAL.Models.JobDefinition
                    {
                        JobId = jobDefinition.JobId,
                        UserId = jobDefinition.UserId,
                        JobName = jobDefinition.JobName,
                        JobDescription = jobDefinition.JobDescription,
                        JobParameters = jobDefinition.JobParameters,
                        Status = jobDefinition.Status,
                        Timestamps = jobDefinition.Timestamps
                    };

                    status = _repository.UpdateJobDefinition(updatedJobDefinition);
                    if (status)
                        return Ok("Job Definition updated successfully");
                    else
                        return BadRequest("Failed to Update Job Definition");
                }
                else
                {
                    return BadRequest("Invalid Data");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }
        [HttpDelete]
        public IActionResult DeleteJobDefinition(string jobId)
        {
            bool status = false;
            try
            {
                if (string.IsNullOrWhiteSpace(jobId))
                {
                    return BadRequest("Job ID cannot be null or empty.");
                }

                status = _repository.DeleteJobDefinition(jobId);
                if (status)
                    return Ok("Job Definition deleted successfully");
                else
                    return BadRequest("Failed to Delete Job Definition");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }
    }
}
