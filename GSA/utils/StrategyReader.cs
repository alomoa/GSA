using GSA.models;
using System.Security.Cryptography.X509Certificates;

namespace GSA.utils
{
    public class StrategyReader
    {
        public List<Strategy> _strategies;

        public StrategyReader()
        {

        }

        public List<Strategy> Read()
        {
            _strategies = InitialiseStrategies(_strategies);
            var pnls = ReadPnls().ToDictionary(x => x.StratName);
            var capitals = ReadCapitals().ToDictionary(x => x.StratName);

            foreach(var strategy in _strategies)
            {
                strategy.Pnl = pnls[strategy.StratName].Pnl;
                strategy.Capital = pnls[strategy.StratName].Capital;
            }

            return _strategies;
        }

        private List<Strategy> ReadPnls()
        {
            var lines = File.ReadAllLines("files/pnl.csv");
            var strategiesWithNames = GetStrategyNames(lines);
            return ReadData<Pnl>(lines, strategiesWithNames, (amount, date) => new Pnl() { Amount=amount, Date= date}, strategy => strategy.Pnl);
        }

        private List<Strategy> ReadCapitals()
        {
            var lines = File.ReadAllLines("files/capital.csv");
            var strategiesWithNames = GetStrategyNames(lines);
            return ReadData<Capital>(lines, strategiesWithNames, (amount, date) => new Capital() { Amount = amount, Date = date }, strategy => strategy.Capital);

        }
        private List<Strategy> GetStrategyNames(string[] lines)
        {
            var headers = lines[0].Split(",").Skip(1).ToArray();
            var strategies = CreateStrategies(headers);
            return strategies;
        }

        private List<Strategy> CreateStrategies(string[] headers)
        {
            var strategies = new List<Strategy>();
            foreach (var line in headers)
            {
                strategies.Add(new Strategy() { StratName = line });
            }

            return strategies;
        }

        public List<Strategy> ReadData<T>(string[] lines, List<Strategy> strategies, Func<decimal, DateTime, T> CreateFunction, Func<Strategy, ICollection<T>> GoToList )
        {
            var stratNames = lines[0]
                .Split(",")
                .Skip(1)
                .ToArray();

            var body = lines
                .Skip(1)
                .Select(row => row.Split(","))
                .ToArray();

            for (int i = 0; i < body.Length; i++)
            {             
                var row = body[i];
                var date = DateTime.Parse(row[0]);
                var amounts = row.Skip(1).ToArray();

                for(int j = 0; j < amounts.Length; j++)
                {
                    var stratName = stratNames[j];
                    var amount = Decimal.Parse(amounts[j]);
                    var current = CreateFunction(amount, date);
                    var currentStrategy = strategies.Where(x => x.StratName == stratName).First();
                    GoToList(currentStrategy).Add(current);
                }
            }

            return strategies;
        }

        private List<Strategy> InitialiseStrategies(List<Strategy> strategies)
        {
            var lines = File.ReadAllLines("files/properties.csv");
            return ParseProperties(lines);
        }

        private List<Strategy> ParseProperties(string[] lines)
        {
            var result = new List<Strategy>();
            var body = lines.Skip(1).ToArray();
            foreach (var row in body)
            {
                var rows = row.Split(',').ToArray();
                if (rows.Length != 2 ) {
                    throw new Exception("Did not receive the correct number of rows");
                }
                var strategy = new Strategy() {
                    StratName = rows[0],
                    Region = rows[1]
                };
                result.Add(strategy);
            }
            return result;
        }
    }
}
