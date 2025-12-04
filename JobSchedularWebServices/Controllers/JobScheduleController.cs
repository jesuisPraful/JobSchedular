using JobSchedularDAL;
using JobSchedularDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobScheduleController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public JobScheduleController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddJobSchedule(Models.JobSchedule jobSchedule)
        {
            bool status = false;
            try
            {
                if (ModelState.IsValid)
                {

                    JobSchedule jobSchedule1 = new JobSchedule
                    {
                        JobId = jobSchedule.JobId,
                        ScheduledExecutionTime = jobSchedule.ScheduledExecutionTime,
                        SchedulePattern = jobSchedule.SchedulePattern,
                        NextRunTime = jobSchedule.NextRunTime,
                        Status = jobSchedule.Status
                    };

                    status = _repository.AddJobSchedule(jobSchedule1);
                    if (status)
                        return Ok("Job Schedule added successfully");
                    else
                        return BadRequest("Failed to Add Job Schedule");
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
        public IActionResult GetJobSchedules()
        {
            try
            {
                var jobSchedules = _repository.GetJobSchedules();
                if(jobSchedules == null || jobSchedules.Count == 0)
                {
                    return NotFound("No Job Schedules Found");
                }
                return Ok(jobSchedules);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpGet("jobId/{jobId}")]
        public IActionResult GetJobScheduleByJobId(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return BadRequest("Job ID cannot be null or empty");
            }
            try
            {
                var jobSchedule = _repository.GetJobScheduleByJobId(jobId);
                if (jobSchedule == null)
                {
                    return NotFound($"Job Schedule with Job ID: {jobId} not found");
                }
                return Ok(jobSchedule);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPut]
        public IActionResult UpdateJobSchedule(Models.JobSchedule jobSchedule)
        {
            bool status = false;
            try
            {
                if (ModelState.IsValid)
                {
                    JobSchedule jobSchedule1 = new JobSchedule
                    {
                        JobId = jobSchedule.JobId,
                        ScheduledExecutionTime = jobSchedule.ScheduledExecutionTime,
                        Status = jobSchedule.Status
                    };

                    status = _repository.UpdateJobSchedule(jobSchedule1);
                    if (status)
                        return Ok("Job Schedule updated successfully");
                    else
                        return BadRequest("Failed to Update Job Schedule");
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

        [HttpDelete("jobId/{jobId}")]
        public IActionResult DeleteJobSchedule(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return BadRequest("Job ID cannot be null or empty");
            }
            bool status = false;
            try
            {
                    status = _repository.DeleteJobSchedule(jobId);
                    if (status)
                        return Ok("Job Schedule deleted successfully");
                    else
                        return BadRequest("Failed to Delete Job Schedule");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }
    }
}
