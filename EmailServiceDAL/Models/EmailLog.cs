using System;
using System.Collections.Generic;

namespace EmailServiceDAL.Models;

public partial class EmailLog
{
    public Guid Id { get; set; }

    public Guid EmailId { get; set; }

    public string? ProviderResponse { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTime LoggedAt { get; set; }

    public virtual Email Email { get; set; } = null!;
}
