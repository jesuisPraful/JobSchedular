using System;
using System.Collections.Generic;

namespace JobSchedularDAL.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<JobDefinition> JobDefinitions { get; set; } = new List<JobDefinition>();
}
