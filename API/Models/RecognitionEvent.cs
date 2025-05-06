using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RecognitionEvent
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int CustomerId { get; set; }

    public int ContractId { get; set; }

    public decimal? Amount { get; set; }

    public virtual Contract Contract { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}