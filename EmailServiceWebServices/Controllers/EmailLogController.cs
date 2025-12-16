using EmailServiceDAL;
using EmailServiceDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailServiceWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailLogController : ControllerBase
    {
        private readonly IEmailService _repository;
        public EmailLogController(IEmailService repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> LogEmailAsync(Models.EmailLog emailLog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EmailLog log = new EmailLog
                    {
                        Id = emailLog.Id,
                        EmailId = emailLog.EmailId,
                        ProviderResponse = emailLog.ProviderResponse,
                        ErrorMessage = emailLog.ErrorMessage,
                        LoggedAt = emailLog.LoggedAt
                    };
                    bool status = await _repository.AddEmailLogAsync(log);
                    if (status)
                        return Ok("Email log added successfully");
                    else
                        return BadRequest("Failed to add Email log");
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
        public async Task<IActionResult> GetAllEmailLogsAsync()
        {
            try
            {
                var emailLogs = await _repository.GetEmailLogsAsync();
                return Ok(emailLogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        //[HttpGet("Pending")]
        //public async Task<IActionResult> GetPendingEmailLogsAsync()
        //{
        //    try
        //    {
        //        var emailLogs = await _repository.GetEmailLogsAsync();
                
        //        retur
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
        //    }
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailLogsAsync(Guid id)
        {
            try
            {
                var emailLogs = await _repository.GetEmailLogByLogsIdAsync(id);
                if (emailLogs != null)
                    return Ok(emailLogs);
                else
                    return NotFound("Email logs not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("log/{emailId}")]
        public async Task<IActionResult> GetEmailLogByIdAsync(Guid emailId)
        {
            try
            {
                var emailLog = await _repository.GetEmailLogsByEmailIdAsync(emailId);
                if (emailLog != null && emailLog.Count>0)
                    return Ok(emailLog);
                else
                    return NotFound("Email log not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmailLogsAsync(Models.EmailLog emailLog)
        {
            try
            {
                var status = await _repository.UpdateEmailLogsAsync(new EmailLog
                {
                    Id = emailLog.Id,
                    EmailId = emailLog.EmailId,
                    ProviderResponse = emailLog.ProviderResponse,
                    ErrorMessage = emailLog.ErrorMessage,
                    LoggedAt = emailLog.LoggedAt
                });
                if (status)
                {
                    return Ok("Updated Successfully");
                }
                else
                {
                    return BadRequest("Failed to Update");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmailLogsAsync(Guid id)
        {
            try
            {
                bool status = await _repository.DeleteEmailLogsAsync(id);
                if (status)
                    return Ok("Email log deleted successfully");
                else
                    return BadRequest("Failed to delete Email log");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

    }
}
