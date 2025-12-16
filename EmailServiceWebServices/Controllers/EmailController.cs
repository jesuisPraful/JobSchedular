using EmailServiceDAL;
using EmailServiceDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailServiceWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _repository;
        public EmailController(IEmailService repository)
        {
            _repository = repository;
        }
        [HttpPost]
        public async Task<IActionResult> SendEmailAsync(Models.Email email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Email email1 = new Email
                    {
                        Id = email.Id,
                        ToEmail = email.ToEmail,
                        Cc = email.Cc,
                        Bcc = email.Bcc,
                        Subject = email.Subject,
                        Body = email.Body,
                        TemplateId = email.TemplateId,
                        Status = email.Status,
                        RetryCount = email.RetryCount,
                        CreatedAt = email.CreatedAt,
                        SentAt = email.SentAt

                    };
                    bool status = await _repository.AddEmailAsync(email1);
                    if (status)
                        return Ok("Email sent successfully");
                    else
                        return BadRequest("Failed to send Email");
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
        public async Task<IActionResult> GetAllEmailsAsync()
        {
            try
            {
                var emails = await _repository.GetEmailsAsync();
                return Ok(emails);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailByIdAsync(Guid id)
        {
            try
            {
                var email = await _repository.GetEmailByIdAsync(id);
                if (email != null)
                    return Ok(email);
                else
                    return NotFound("Email not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmailAsync(Models.Email email)
        {
            try
            {
               var status = await _repository.UpdateEmailAsync(new Email
               {
                   Id = email.Id,
                   ToEmail = email.ToEmail,
                   Cc = email.Cc,
                   Bcc = email.Bcc,
                   Subject = email.Subject,
                   Body = email.Body,
                   TemplateId = email.TemplateId,
                   Status = email.Status,
                   RetryCount = email.RetryCount,
                   CreatedAt = email.CreatedAt,
                   SentAt = email.SentAt
               });
                if (status)
                    return Ok("Email updated successfully");
                else
                    return BadRequest("Failed to update Email");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailAsync(Guid id)
        {
            try
            {
                bool status = await _repository.DeleteEmail(id);
                if (status)
                    return Ok("Email deleted successfully");
                else
                    return NotFound("Email not found");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }
         
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingEmails()
        {
            try
            {
                var emails = await _repository.GetPendingEmailAsync();

                if (emails == null || !emails.Any())
                    return NoContent();

                return Ok(emails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateEmailStatus(Guid id,string status,int retryCount,DateTime dateTime)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _repository.UpdateEmailStatusAsync(id,status,retryCount,dateTime);

                if (!result)
                    return NotFound("Email not found");

                return Ok("Email status updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
