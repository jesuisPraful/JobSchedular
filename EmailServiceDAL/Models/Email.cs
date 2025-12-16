using System;
using System.Collections.Generic;

namespace EmailServiceDAL.Models;

public partial class Email
{
    public Guid Id { get; set; }

    public string ToEmail { get; set; } = null!;

    public string? Cc { get; set; }

    public string? Bcc { get; set; }

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public Guid? TemplateId { get; set; }

    public string Status { get; set; } = null!;

    public int RetryCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? SentAt { get; set; }

    public virtual ICollection<EmailLog> EmailLogs { get; set; } = new List<EmailLog>();
}
