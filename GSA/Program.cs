namespace gsa
{
    public class program
    {
        public static void Main(string[] args)
        {
            PnLReader pnLReader = new();

            List<StrategyPnl> strategyPnls = pnLReader.Read("pnl.csv");            
        }
    }
}