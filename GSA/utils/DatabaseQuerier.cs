using GSA.Data.Context;
using GSA.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GSA.utils
{
    // TODO: Single responsiblity principle. Decouple the code! 
    public class DatabaseQuerier
    {
        
        private readonly StrategyContext _dbContext;

        public DatabaseQuerier(StrategyContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Data.Entity.Strategy> QueryCapitals(string[] strategyNames)
        {
            if (!strategyNames.Any()) return new List<Strategy>();

            var strategies = GetStrategiesWithCapitals(strategyNames);
            if (!strategies.Any()) return new List<Strategy>();

            var cumulativeStrategies = CumulateStrategyCapitals(strategies);

            return cumulativeStrategies;
        }

        public List<CumlativeStrategyCapitals> CumulateStrategyCapitals(List<Strategy> strategies)
        {
            // I don't like that you reused the class here
            var cumulativeStrategies = new List<GSA.Data.Entity.Strategy>();

            foreach (var strategy in strategies)
            {
                var cumulativeStrategy = new GSA.Data.Entity.Strategy();
                cumulativeStrategy.StratName = strategy.StratName;

                var total = 0.0M;
                foreach (var capital in strategy.Capitals)
                {
                    total = capital.Amount + total;

                    // I don't like that you reused the class here
                    var cumulativeCapital = new CumlativeCapital() { Date = capital.Date, Amount = total };
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
            // or can use _dbContext. Pick one way, don't mix
            using (var db = new StrategyContext())
            {
                var strategies = db.Strategies.Where(x => strategyNames.Contains(x.StratName))
                    .Include(x => x.Capitals)
                    .ToList();

                foreach (var strategy in strategies)
                {
                    strategy.Capitals = strategy.Capitals.OrderBy(x => x.Date).ToList();
                }
                
                return strategies;
            }
    
        }

        private List<GSA.Data.Entity.Strategy> GetStrategiesWithPnlsFromRegion(string region)
        {
            var strategies = new List<GSA.Data.Entity.Strategy>();

            strategies = _dbContext.Strategies.Where(x => x.Region == region)
                .Include(x => x.Pnls)
                .ToList();
            
            return strategies;
        }
    }
}
