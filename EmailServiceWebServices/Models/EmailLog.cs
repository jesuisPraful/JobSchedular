using System.ComponentModel.DataAnnotations;

namespace EmailServiceWebServices.Models
{
    public class EmailLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public Guid EmailId { get; set; }

        public string? ProviderResponse { get; set; }

        public string? ErrorMessage { get; set; }

        [Required]
        public DateTime LoggedAt { get; set; }

        public virtual Email Email { get; set; } = null!;
    }
}
