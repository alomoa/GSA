using Microsoft.VisualBasic.FileIO;

namespace gsa
{
    public class PnLReader
    {
        public PnLReader()
        {
        }

        public List<StrategyPnl> Read(string path)
        {
            List<StrategyPnl> strategyPnls = new List<StrategyPnl>();
            using (TextFieldParser parser = new TextFieldParser(path))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                string[] headers = parser.ReadFields();
                headers = headers.Skip(1).ToArray();

                SetupStrategyPnL(headers, strategyPnls);
                AddFields(parser, strategyPnls);
            }
            return strategyPnls;
        }

        private void AddFields(TextFieldParser parser, List<StrategyPnl> strategyPnls)
        {
            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();

                var date = fields[0];
                fields = fields.Skip(1).ToArray();

                for (var i = 0; i < fields.Length; i++)
                {
                    var pnl = new Pnl
                    {
                        Amount = decimal.Parse(fields[i]),
                        Date = DateTime.Parse(date)
                    };
                    strategyPnls[i].Pnls.Add(pnl);
                }
            }
        }

        private void SetupStrategyPnL(string[] headers, List<StrategyPnl> strategyPnls)
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