using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class JobSchedule
    {

        [Key]
        public string JobId { get; set; } = null!;

        [Required]
        public DateTime ScheduledExecutionTime { get; set; }

        [Required]
        public string? SchedulePattern { get; set; }

        [Required]
        public DateTime? NextRunTime { get; set; }

        [Required]
        public string? Status { get; set; }
    }
}
