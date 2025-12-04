using System.ComponentModel.DataAnnotations;

namespace JobSchedularWebServices.Models
{
    public class ExecutionNode
    {
        [Key]
        public string NodeId { get; set; } = null!;

        [Required]
        public string NodeName { get; set; } = null!;

        [Required]
        public string NodeIpaddress { get; set; } = null!;

        [Required]
        public string? NodeStatus { get; set; }
    }
}
