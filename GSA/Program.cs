using GSA.Data.Scaffolded;

namespace gsa
{
    public class program
    {
        public static void Main(string[] args)
        {
            PnLReader pnLReader = new();

            List<StrategyPnl> strategyPnls = pnLReader.Execute("pnl.csv");
            foreach (var strategy in strategyPnls)
            {
                Console.WriteLine(strategy.Strategy);
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
                        Strategy = strategy.Strategy,
                    };
                    db.StrategyPnls.Add(dBStrategyPnl);
                    db.SaveChanges();

                    var dBPnls = new List<GSA.Data.Entity.Pnl>();
                    foreach (var pnl in strategy.Pnls)
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