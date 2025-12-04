using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class ResourceAllocation
    {
        [Key]
        public string AllocationId { get; set; } = null!;

        [Required]
        public string JobId { get; set; } = null!;

        [Required]
        public string ExecutionNodeId { get; set; } = null!;

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime? EndTime { get; set; }
    }
}
