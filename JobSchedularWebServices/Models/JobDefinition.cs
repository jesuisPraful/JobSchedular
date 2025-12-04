using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class JobDefinition
    {
        [Key]
        public string JobId { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string JobName { get; set; } = null!;


        public string? JobDescription { get; set; }


        public string? JobParameters { get; set; }

        public string? Status { get; set; }

        public DateTime? Timestamps { get; set; }
    }
}
