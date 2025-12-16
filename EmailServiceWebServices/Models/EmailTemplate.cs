using System.ComponentModel.DataAnnotations;

namespace EmailServiceWebServices.Models
{
    public class EmailTemplate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Subject { get; set; } = null!;

        [Required]
        public string Body { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
