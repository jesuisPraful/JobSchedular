using EmailServiceDAL;
using EmailServiceDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailServiceWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OutboxEmailController : ControllerBase
    {
        private readonly IEmailService _repository;
        public OutboxEmailController(IEmailService repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> AddOutboxEmail(Models.OutboxEmail outboxEmail)
        {
            try
            {
                var status = await _repository.AddOutboxEmailAsync(new OutboxEmail
                {
                    Id = outboxEmail.Id,
                    Payload = outboxEmail.Payload,
                    Status = outboxEmail.Status,
                    CreatedAt = outboxEmail.CreatedAt,
                    ProcessedAt = outboxEmail.ProcessedAt
                });

                if(status)
                {
                    return Ok("Added Successfully");
                }
                else{
                    return BadRequest("Failed to add OutboxEmail");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Server Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOutBoxEmails()
        {
            try
            {
                var outboxEmails = await _repository.GetOutboxEmailsAsync();
                if (outboxEmails == null || outboxEmails.Count == 0)
                {
                    return BadRequest("OutBoxEmail is Null");
                }
                else
                {
                    return Ok(outboxEmails);
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        [HttpGet("{id}")]   
        public async Task<IActionResult> GetOutBoxEmailAsync(Guid id)
        {
            try
            {
                var outboxEmail = await _repository.GetOutboxEmailByIdAsync(id);
                if(outboxEmail != null)
                {
                    return Ok(outboxEmail);
                }
                else
                {
                    return NotFound("OutboxEmail not found");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOutboxEmail(Models.OutboxEmail outboxEmail)
        {
            try
            {
                var status = await _repository.UpdateOutboxEmailAsync(new OutboxEmail
                {
                    Id = outboxEmail.Id,
                    Payload = outboxEmail.Payload,
                    Status = outboxEmail.Status,
                    CreatedAt = outboxEmail.CreatedAt,
                    ProcessedAt = outboxEmail.ProcessedAt
                });

                if (status)
                {
                    return Ok("Updated Successfully");
                }
                else
                {
                    return BadRequest("Failed to Update OutboxEmail");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteOutBoxEmail(Guid id)
        {
            try
            {
                var status = await _repository.DeleteOutboxEmailAsync(id);
                if (status)
                {
                    return Ok("Deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete OutboxEmail");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
     }
}
