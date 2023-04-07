using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Net.Http.Headers;
using static gsa.program;

namespace gsa
{
    public class program
    {
        public static void Main(string[] args)
        {
            List<StrategyPnl> strategyPnls = new List<StrategyPnl>();

            using (TextFieldParser parser = new TextFieldParser("pnl.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                
                string[] headers = parser.ReadFields();
                headers = headers.Skip(1).ToArray();

                

                for (var i = 0; i < headers.Length; i++)
                {
                    var strategyPnln = new StrategyPnl
                    {
                        Strategy = headers[i],
                        Pnls = new List<Pnl>()
                    };
                    strategyPnls.Add(strategyPnln);
                }

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
        }
    }
}