using System;
using System.Collections.Generic;

namespace API.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();

}
