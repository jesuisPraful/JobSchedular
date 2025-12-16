using System;
using System.Collections.Generic;

namespace EmailServiceDAL.Models;

public partial class EmailTemplate
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
