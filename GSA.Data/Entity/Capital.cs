using System;
using System.Collections.Generic;

namespace GSA.Data.Entity;

public partial class Capital
{
    public int CapitalId { get; set; }

    public int StrategyId { get; set; }

    public DateTime Date { get; set; }

    public decimal Amount { get; set; }

    public virtual Strategy Strategy { get; set; } = null!;
}
