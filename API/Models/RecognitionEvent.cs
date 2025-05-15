using System;
using System.Collections.Generic;

namespace API.Models;

public partial class RecognitionEvent
{
    public int Id { get; set; }
    public int ContractId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime Date { get; set; }

    public virtual Contract Contract { get; set; } = null!;

}