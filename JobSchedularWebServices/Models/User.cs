using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
