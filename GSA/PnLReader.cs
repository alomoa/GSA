using System.Runtime.CompilerServices;

namespace gsa
{
    public class PnLReader
    {
        public PnLReader()
        {
        }

        public List<StrategyPnl> Execute(string path)
        {
            var lines = File.ReadAllLines(path);
            var strategyPnl = Read(lines);

            return strategyPnl;
        }
        public List<StrategyPnl> Read(string[] lines)
        {
            List<StrategyPnl> strategyPnls = new List<StrategyPnl>();
            var headers = lines[0].Split(",").Skip(1).ToArray();
            var body = lines
                .Skip(1)
                .Select(row => row.Split(","))
                .ToArray();
            SetupStrategyPnL(headers, strategyPnls);
            AddFields(body, strategyPnls);

            return strategyPnls;
        }

        public void AddFields(string[][] body, List<StrategyPnl> strategyPnls)
        {
            for(int i = 0; i < body.Length; i++)
            {
                string[] row = body[i];

                var date = row[0];
                row = row.Skip(1).ToArray();

                for (var j = 0; j < row.Length; j++)
                {
                    var pnl = new Pnl
                    {
                        Amount = decimal.Parse(row[j]),
                        Date = DateTime.Parse(date)
                    };
                    strategyPnls[j].Pnls.Add(pnl);
                }
            }
        }

        public void SetupStrategyPnL(string[] headers, List<StrategyPnl> strategyPnls)
        {
            for (var i = 0; i < headers.Length; i++)
            {
                var strategyPnl = new StrategyPnl
                {
                    Strategy = headers[i],
                    Pnls = new List<Pnl>()
                };
                strategyPnls.Add(strategyPnl);
            }
        }
    }
}