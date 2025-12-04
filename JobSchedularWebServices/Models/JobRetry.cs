using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class JobRetry
    {
        [Key]
        public string RetryId { get; set; } = null!;

        [Required]
        public string JobId { get; set; } = null!;

        [Required]
        public int RetryAttemptNumber { get; set; }

        [Required]
        public string? RetryStatus { get; set; }

        [Required]
        public DateTime RetryTime { get; set; }
    }
}
