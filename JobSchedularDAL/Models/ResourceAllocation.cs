using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class ResourceAllocation
{
    public string AllocationId { get; set; } = null!;

    public string JobId { get; set; } = null!;

    public string ExecutionNodeId { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public virtual ExecutionNode ExecutionNode { get; set; } = null!;

    public virtual JobDefinition Job { get; set; } = null!;
}
