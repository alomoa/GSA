using GSA.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.utils
{
    public class DatabaseQuerier
    {
        public DatabaseQuerier() { }

        public List<Data.Entity.Strategy> QueryCapitals(string[] strategyNames)
        {


            if(strategyNames.Length == 0) throw new NullReferenceException();

            var strategies = GetStrategies(strategyNames);

            var cumulativeStrategies = new List<GSA.Data.Entity.Strategy>();

            foreach( var strategy in strategies)
            {
                var cumulativeStrategy = new GSA.Data.Entity.Strategy();
                cumulativeStrategy.StratName = strategy.StratName;

                var total = 0.0M;
                foreach(var capital in  strategy.Capitals) {
                    total = capital.Amount + total;
                    var cumulativeCapital = new GSA.Data.Entity.Capital() { Date = capital.Date, Amount = total };
                    cumulativeStrategy.Capitals.Add(cumulativeCapital);
                }
                cumulativeStrategies.Add(cumulativeStrategy);
            }

            return cumulativeStrategies;
        }


        private List<GSA.Data.Entity.Strategy> GetStrategies(string[] strategyNames)
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
    }
}
