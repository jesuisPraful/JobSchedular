namespace EmailServiceWebServices.Models
{
    public class OutboxEmail
    {
        public Guid Id { get; set; }

        public string Payload { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}
