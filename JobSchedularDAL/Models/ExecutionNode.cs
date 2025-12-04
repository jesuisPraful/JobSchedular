using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class ExecutionNode
{
    public string NodeId { get; set; } = null!;

    public string NodeName { get; set; } = null!;

    public string NodeIpaddress { get; set; } = null!;

    public string? NodeStatus { get; set; }

    public virtual ICollection<ResourceAllocation> ResourceAllocations { get; set; } = new List<ResourceAllocation>();
}
