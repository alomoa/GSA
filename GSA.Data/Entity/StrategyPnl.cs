using System;
using System.Collections.Generic;

namespace GSA.Data.Entity;

public partial class StrategyPnl
{
    public int Id { get; set; }

    public string? Strategy { get; set; }

    public virtual ICollection<Pnl> Pnls { get; set; } = new List<Pnl>();
}
