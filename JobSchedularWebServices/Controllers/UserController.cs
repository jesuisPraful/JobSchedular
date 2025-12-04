using JobSchedularDAL;
using JobSchedularDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSchedularWebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IJobSchedular _repository;

        public UserController(IJobSchedular repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult AddUser(Models.User user)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    User newUser = new User
                    {
                        UserId = Guid.NewGuid().ToString(),
                        //UserId = user.UserId,
                        Username = user.Username,
                        Email = user.Email,
                        Password = user.Password
                    };

                    var status = _repository.AddUser(newUser);
                    if (status)
                        return Ok("User added successfully");
                    else
                        return BadRequest("Failed to Add User");
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
        public IActionResult GetUsers()
        {
            try
            {
                var usersList = _repository.GetUsers();

                if (usersList == null)
                    return NotFound("No users found");

                return Ok(usersList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error");
            }
        }

        [HttpGet("{userId}")]
        public IActionResult GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            try
            {
                var user = _repository.GetUserById(userId);

                if (user == null)
                {
                    return NotFound($"User with ID '{userId}' not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Ideally use ILogger instead of Console.WriteLine
                Console.WriteLine($"Error occurred while retrieving user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error occurred");
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email cannot be null or empty.");
            }

            try
            {
                var user = _repository.GetUserByEmail(email);

                if (user == null)
                {
                    return NotFound($"User with email '{email}' not found.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                // Ideally use ILogger instead of Console.WriteLine
                Console.WriteLine($"Error occurred while retrieving user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error occurred");
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(string userId) {
            if (string.IsNullOrWhiteSpace(userId)) {
                return BadRequest("User id is null");
            }
            try
            {
                var status = _repository.DeleteUser(userId);
                if(status == true)
                {
                    return Ok("User Deleted");
                }
                else
                {
                    return NotFound("User Not Found");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,"Server error occurred");
            }

        }
    }
}
