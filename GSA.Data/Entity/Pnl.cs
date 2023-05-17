using System;
using System.Collections.Generic;

namespace GSA.Data.Entity;

public partial class Pnl
{
    public int Id { get; set; }

    public int StrategyPnlId { get; set; }

    public DateTime? Date { get; set; }

    public decimal? Amount { get; set; }

    public virtual StrategyPnl StrategyPnl { get; set; } = null!;
}
