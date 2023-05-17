namespace GSA
{
    public class PnLReader
    {
        public PnLReader()
        {
        }

        public List<Strategy> ReadAndPopuplate()
        {
            var lines = File.ReadAllLines("pnl.csv");
            var strategyPnl = Read(lines);

            return strategyPnl;
        }
        public List<Strategy> Read(string[] lines)
        {
            List<Strategy> strategyPnls = new List<Strategy>();
            var headers = lines[0].Split(",").Skip(1).ToArray();
            var body = lines
                .Skip(1)
                .Select(row => row.Split(","))
                .ToArray();
            SetupStrategyPnL(headers, strategyPnls);
            AddFields(body, strategyPnls);

            return strategyPnls;
        }

        public void AddFields(string[][] body, List<Strategy> strategyPnls)
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
                    strategyPnls[j].Pnl.Add(pnl);
                }
            }
        }

        public void SetupStrategyPnL(string[] headers, List<Strategy> strategyPnls)
        {
            for (var i = 0; i < headers.Length; i++)
            {
                var strategyPnl = new Strategy
                {
                    StratName = headers[i],
                    Pnl = new List<Pnl>()
                };
                strategyPnls.Add(strategyPnl);
            }
        }
    }
}