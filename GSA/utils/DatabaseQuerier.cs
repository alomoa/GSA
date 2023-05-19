using GSA.Data.Context;
using GSA.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GSA.utils
{
    public class DatabaseQuerier
    {
        DbContext _dbContext;
        public DatabaseQuerier(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Data.Entity.Strategy> QueryCapitals(string[] strategyNames)
        {
            if (strategyNames.Length == 0) throw new NullReferenceException();

            var strategies = GetStrategiesWithCapitals(strategyNames);

            var cumulativeStrategies = cumulateStrategyCapitals(strategies);

            return cumulativeStrategies;
        }

        public List<Data.Entity.Strategy> cumulateStrategyCapitals(List<Strategy> strategies)
        {
            var cumulativeStrategies = new List<GSA.Data.Entity.Strategy>();

            foreach (var strategy in strategies)
            {
                var cumulativeStrategy = new GSA.Data.Entity.Strategy();
                cumulativeStrategy.StratName = strategy.StratName;

                var total = 0.0M;
                foreach (var capital in strategy.Capitals)
                {
                    total = capital.Amount + total;
                    var cumulativeCapital = new GSA.Data.Entity.Capital() { Date = capital.Date, Amount = total };
                    cumulativeStrategy.Capitals.Add(cumulativeCapital);
                }
                cumulativeStrategies.Add(cumulativeStrategy);
            }

            return cumulativeStrategies;
        }

        public Dictionary<DateTime, decimal> QueryPnls(string region)
        {
            if (region.IsNullOrEmpty()) throw new NullReferenceException();

            var strategies = GetStrategiesWithPnlsFromRegion(region);

            var pnlDict = cumulateStrategyPnls(strategies);

            return pnlDict;

        }

        public Dictionary<DateTime, decimal> cumulateStrategyPnls(List<Strategy> strategies)
        {
            var pnlDict = new Dictionary<DateTime, decimal>();

            foreach (var strategy in strategies)
            {
                var cumulativeStrategy = new GSA.Data.Entity.Strategy();
                cumulativeStrategy.StratName = strategy.StratName;

                var currentTotal = 0.0M;
                var total = 0.0M;
                var currentDate = strategy.Pnls.First().Date;
                foreach (var pnl in strategy.Pnls)
                {
                    if (pnl.Date.Month != currentDate.Month)
                    {
                        total += currentTotal;

                        if (pnlDict.ContainsKey(currentDate))
                        {
                            pnlDict[currentDate] = pnlDict[currentDate] + total;
                        }
                        else
                        {
                            pnlDict[currentDate] = total;
                        }
                        currentDate = pnl.Date;
                        currentTotal = 0.0m;
                    }
                    currentTotal += pnl.Amount;
                }
                total += currentTotal;

                if (pnlDict.ContainsKey(currentDate))
                {
                    pnlDict[currentDate] = pnlDict[currentDate] + total;
                }
                else
                {
                    pnlDict[currentDate] = total;
                }


            }

            return pnlDict;
        }

        private List<GSA.Data.Entity.Strategy> GetStrategiesWithCapitals(string[] strategyNames)
        {
            var strategies = new List<GSA.Data.Entity.Strategy>();

            foreach (var stratName in strategyNames)
            {
                using (var db = new StrategyContext())
                {
                    var strategy = db.Strategies.Where(x => x.StratName == stratName)
                        .Include(x => x.Capitals)
                        .First();
                    strategy.Capitals.OrderBy(x => x.Date).ToList();
                    strategies.Add(strategy);
                }
            }

            return strategies;
        }

        private List<GSA.Data.Entity.Strategy> GetStrategiesWithPnlsFromRegion(string region)
        {
            var strategies = new List<GSA.Data.Entity.Strategy>();

            using (var db = new StrategyContext())
            {
                strategies = db.Strategies.Where(x => x.Region == region)
                    .Include(x => x.Pnls)
                    .ToList();
            }

            return strategies;
        }
    }
}
