using GSA.models;

namespace GSA.utils
{
    public class CapitalReader
    {
        public List<Strategy> ReadAndPopulate(List<Strategy> strategies)
        {
            var lines = File.ReadAllLines("files/Capital.csv");
            Read(lines, strategies);

            return strategies;
        }

        public void Read(string[] lines, List<Strategy> strategies)
        {
            var headers = lines[0].Split(",").Skip(1).ToArray();
            var body = lines
                .Skip(1)
                .Select(row => row.Split(","))
                .ToArray();

            AddFields(body, headers, strategies);
        }

        private void AddFields(string[][] body, string[] headers, List<Strategy> strategies)
        {
            for (int i = 0; i < body.Length; i++)
            {
                string[] row = body[i];

                var date = row[0];
                row = row.Skip(1).ToArray();

                for (var j = 0; j < row.Length; j++)
                {
                    var header = headers[j];
                    var strategy = strategies.Where(x => x.StratName == header).First();

                    var capital = new Capital
                    {
                        Amount = decimal.Parse(row[j]),
                        Date = DateTime.Parse(date)
                    };
                    strategy.Capital.Add(capital);
                }
            }
        }
    }

}
