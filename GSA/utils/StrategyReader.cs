using GSA.models;

namespace GSA.utils
{
    public class StrategyReader
    {
        public List<Strategy> _strategies;

        public StrategyReader()
        {
            var pnlReader = new PnLReader();
            var capitalReader = new CapitalReader();
            var propertiesReader = new PropertiesReader();

            _strategies = pnlReader.ReadAndPopuplate();
            capitalReader.ReadAndPopulate(_strategies);
            propertiesReader.ConvertProperties(_strategies);
        }
    }
}
