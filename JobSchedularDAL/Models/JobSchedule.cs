using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class JobSchedule
{
    public string JobId { get; set; } = null!;

    public DateTime ScheduledExecutionTime { get; set; }

    public string? SchedulePattern { get; set; }

    public DateTime? NextRunTime { get; set; }

    public string? Status { get; set; }

    public virtual JobDefinition Job { get; set; } = null!;
}
