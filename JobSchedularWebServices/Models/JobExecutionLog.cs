using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class JobExecutionLog
    {
        [Key]
        public string ExecutionLogId { get; set; } = null!;

        [Required]
        public string JobId { get; set; } = null!;

        [Required]
        public string? ExecutionStatus { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime? EndTime { get; set; }

        [Required]
        public string ExecutionNodeId { get; set; } = null!;
    }
}
