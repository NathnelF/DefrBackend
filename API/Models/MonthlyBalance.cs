using System;
using System.Collections.Generic;

namespace API.Models;

public partial class MonthlyBalance
{
    public int Id { get; set; }

    public DateTime? Month { get; set; }

    public DateTime? Year { get; set; }

    public decimal? Balance { get; set; }
}