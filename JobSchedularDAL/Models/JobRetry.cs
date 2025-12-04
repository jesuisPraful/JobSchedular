using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class JobRetry
{
    public string RetryId { get; set; } = null!;

    public string JobId { get; set; } = null!;

    public int RetryAttemptNumber { get; set; }

    public string? RetryStatus { get; set; }

    public DateTime RetryTime { get; set; }

    public virtual JobDefinition Job { get; set; } = null!;
}
