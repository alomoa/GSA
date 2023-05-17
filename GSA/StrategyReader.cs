using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA
{
    public class StrategyReader
    {
        public List<Strategy> _strategies;

        public StrategyReader() { 
            var pnlReader = new PnLReader();
            var capitalReader = new CapitalReader();
            var propertiesReader = new PropertiesReader();

            _strategies = pnlReader.ReadAndPopuplate();
            capitalReader.ReadAndPopulate(_strategies);
            propertiesReader.ConvertProperties(_strategies);
        }
    }
}
