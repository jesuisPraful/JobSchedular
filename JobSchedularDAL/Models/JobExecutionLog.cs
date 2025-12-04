using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class JobExecutionLog
{
    public string ExecutionLogId { get; set; } = null!;

    public string JobId { get; set; } = null!;

    public string? ExecutionStatus { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string ExecutionNodeId { get; set; } = null!;

    public virtual JobDefinition Job { get; set; } = null!;
}
