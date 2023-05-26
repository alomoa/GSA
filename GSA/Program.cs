using GSA.Data.Context;
using GSA.models;
using GSA.utils;
using System.Collections.Generic;

namespace GSA
{
    public class program
    {
        public static void Main(string[] args)
        {
            var strategyReader = new StrategyReader();
            var result = strategyReader.Read();
            foreach ( var item in result )
            {
                Console.WriteLine(item);
            }
            SaveToDatabase(result);

            DatabaseQuerier _databaseQuerier = new DatabaseQuerier(new StrategyContext());

            ConsoleHelpers consoleHelpers = new ConsoleHelpers(_databaseQuerier);
            consoleHelpers.ProcessCommands();
        }

        private static void SaveToDatabase(List<Strategy> strategies)
        {
            using(var db = new StrategyContext())
            {
                db.RemoveRange(db.Capitals);
                db.RemoveRange(db.Pnls);
                db.RemoveRange(db.Strategies);
                db.SaveChanges();
            }
            foreach(Strategy strategy in strategies)
            {
                using(var db = new StrategyContext())
                {
                    GSA.Data.Entity.Strategy dbStrategies = new();
                    dbStrategies.StratName = strategy.StratName;
                    dbStrategies.Region = strategy.Region;

                    db.Strategies.Add(dbStrategies);
                    db.SaveChanges();

                    List<Data.Entity.Pnl> dbPnls = new();
                    List<Data.Entity.Capital> dbCapitals = new();

                    foreach(var pnl in strategy.Pnl)
                    {
                        var dbPnl = new Data.Entity.Pnl() { 
                            StrategyId = dbStrategies.StrategyId,
                            Date = pnl.Date,
                            Amount = pnl.Amount,
                        };

                        dbPnls.Add(dbPnl);
                    }

                    foreach(var capital in strategy.Capital)
                    {
                        var dbCapital = new Data.Entity.Capital()
                        {
                            StrategyId = dbStrategies.StrategyId,
                            Date = capital.Date,
                            Amount = capital.Amount
                        };
                        
                        dbCapitals.Add(dbCapital);
                    }

                    dbStrategies.Capitals = dbCapitals;
                    dbStrategies.Pnls = dbPnls;
                    db.SaveChanges();
                }
            }
        }
    }
}