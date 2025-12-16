using EmailServiceDAL;
using EmailServiceDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;

namespace EmailServiceWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IEmailService _repository;
        public EmailTemplateController(IEmailService repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmailTemplateAsync(Models.EmailTemplate emailTemplate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newTemplate = new EmailTemplate
                    {
                        Id = Guid.NewGuid(),
                        Name = emailTemplate.Name,
                        Subject = emailTemplate.Subject,
                        Body = emailTemplate.Body,
                        CreatedAt = DateTime.UtcNow
                    };

                    var result = await _repository.AddEmailTemplateAsync(newTemplate);
                    if (result)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest("Failed to create email template.");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the email template.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmailTemplatesAsync()
        {
            try
            {
                var templates = await _repository.GetEmailTemplatesAsync();
                return Ok(templates);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving email templates.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailTemplateByIdAsync(Guid id)
        {
            try
            {
                var template = await _repository.GetEmailTemplateByIdAsync(id);
                if (template != null)
                {
                    return Ok(template);
                }
                else
                {
                    return NotFound($"Email template with ID {id} not found.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the email template.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmailTemplateAsync(Models.EmailTemplate emailTemplate)
        {
            try
            {
                var status = await _repository.UpdateEmailTemplateAsync(new EmailTemplate
                {
                    Id = emailTemplate.Id,
                    Name = emailTemplate.Name,
                    Subject = emailTemplate.Subject,
                    Body = emailTemplate.Body,
                    CreatedAt = emailTemplate.CreatedAt
                });

                if (status)
                {
                    return Ok(status);
                }
                else
                {
                    return BadRequest("Failed to update");
                }

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the email template.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailTemplateAsync(Guid id)
        {
            try
            {
                var result = await _repository.DeleteEmailTemplateAsync(id);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound($"Email template with ID {id} not found.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the email template.");
            }
        }

    }
}
