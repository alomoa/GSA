using GSA.Data.Scaffolded;

namespace GSA
{
    public class program
    {
        public static void Main(string[] args)
        {
            PnLReader pnLReader = new();

            List<Strategy> strategyPnls = pnLReader.Execute("pnl.csv");
            foreach (var strategy in strategyPnls)
            {
                Console.WriteLine(strategy.StratName);
            }

            using (var db = new PnlContext())
            {
                db.Pnls.RemoveRange(db.Pnls);
                db.StrategyPnls.RemoveRange(db.StrategyPnls);
                db.SaveChanges();
            }


            foreach (var strategy in strategyPnls)
            {
                using (var db = new PnlContext())
                {
                    var dBStrategyPnl = new GSA.Data.Entity.StrategyPnl() { 
                        Strategy = strategy.StratName,
                    };
                    db.StrategyPnls.Add(dBStrategyPnl);
                    db.SaveChanges();

                    var dBPnls = new List<GSA.Data.Entity.Pnl>();
                    foreach (var pnl in strategy.Pnl)
                    {
                        var dBPnl = new GSA.Data.Entity.Pnl()
                        {
                            Date = pnl.Date,
                            Amount = pnl.Amount,
                            StrategyPnlId = dBStrategyPnl.Id
                        };
                        db.Pnls.Add(dBPnl);
                        db.SaveChanges();
                        dBPnls.Add(dBPnl);
                    }

                    dBStrategyPnl.Pnls = dBPnls;

                    db.StrategyPnls.Update(dBStrategyPnl);
                    db.SaveChanges();
                }

            }

        }
    }
}