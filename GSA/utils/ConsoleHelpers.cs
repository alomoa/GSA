﻿using GSA.Data.Context;

namespace GSA.utils
{
    public class ConsoleHelpers
    {

        DatabaseQuerier _databaseQuerier = new DatabaseQuerier(new StrategyContext());

        public ConsoleHelpers(DatabaseQuerier databaseQuerier)
        {
            _databaseQuerier = databaseQuerier;
        }


        public void ProcessCommands()
        {
            var command = Console.ReadLine().ToLower();
            ProcessCommands(command);
        }

        // I wouldn't test this
        public void ProcessCommands(string command)
        {
            var commandParts = command.Split(' ');

            if (commandParts[0] == "capital")
            {
                var strategies = commandParts.Skip(1).ToArray();

                ProcessCapital(strategies);

            }
            else if (commandParts[0] == "cumulative-pnl")
            {
                var region = commandParts[1];

                ProcessCumulativePnL(region);
            }
            else
            {
                Console.WriteLine("Invalid command.");
            }
        }

        public void ProcessCapital(string[] strategies)
        {
            
            var results = _databaseQuerier.QueryCapitals(strategies);

            for (int i = 0; i < results[0].Capitals.Count(); i++)
            {

                foreach (var result in results)
                {
                    var capital = result.Capitals.ElementAt(i);

                    Console.WriteLine($"strategy: {result.StratName}, date: {capital.Date.ToString("yyyy-MM-dd")}, capital: {capital.Amount.ToString("0")}");
                }
            }
        }

        public void ProcessCumulativePnL(string region)
        {

            var result = _databaseQuerier.QueryPnls(region);

            var keys = result.Keys;
            keys.OrderBy(x => x.Date).ToList();

            foreach (var key in keys)
            {
                Console.WriteLine($"date: {key.ToString("yyyy-MM-dd")} cululativePnl:{result[key]}");
            }
        }
    }
}
