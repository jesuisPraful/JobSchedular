using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class JobDefinition
{
    public string JobId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string JobName { get; set; } = null!;

    public string? JobDescription { get; set; }

    public string? JobParameters { get; set; }

    public string? Status { get; set; }

    public DateTime? Timestamps { get; set; }

    public virtual ICollection<JobExecutionLog> JobExecutionLogs { get; set; } = new List<JobExecutionLog>();

    public virtual ICollection<JobRetry> JobRetries { get; set; } = new List<JobRetry>();

    public virtual JobSchedule? JobSchedule { get; set; }

    public virtual ICollection<ResourceAllocation> ResourceAllocations { get; set; } = new List<ResourceAllocation>();

    public virtual User User { get; set; } = null!;
}
