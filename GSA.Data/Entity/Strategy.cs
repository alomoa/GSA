using System;
using System.Collections.Generic;

namespace GSA.Data.Entity;

public partial class Strategy
{
    public int StrategyId { get; set; }

    public string StratName { get; set; } = null!;

    public string Region { get; set; } = null!;

    public virtual ICollection<Capital> Capitals { get; set; } = new List<Capital>();

    public virtual ICollection<Pnl> Pnls { get; set; } = new List<Pnl>();
}
