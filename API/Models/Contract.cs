using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Contract
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public int CustomerId { get; set; }

    public decimal Price { get; set; }

    public DateTime? OriginalContractStart { get; set; }

    public DateTime? CurrentTermStart { get; set; }

    public DateTime? CurrentTermEnd { get; set; }
    public DateTime? InvoiceDate { get; set; }

    public int? TermLength { get; set; }

    public bool? IsAutoRenew { get; set; }

    public decimal? RenewalPriceIncrease { get; set; }

    public bool? IsChurned { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<RecognitionEvent> RecognitionEvents { get; set; } = new List<RecognitionEvent>();

    public virtual Service Service { get; set; } = null!;
}