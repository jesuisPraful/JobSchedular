using System.ComponentModel.DataAnnotations;

namespace EmailServiceWebServices.Models
{
    public class Email
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        public string ToEmail { get; set; } = null!;


        public string? Cc { get; set; }

        public string? Bcc { get; set; }

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public string Body { get; set; } = null!;

        public Guid? TemplateId { get; set; }

        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public int RetryCount { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime? SentAt { get; set; }
    }
}
