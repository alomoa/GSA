namespace gsa
{
    public class program
    {
        public static void Main(string[] args)
        {
            PnLReader pnLReader = new();

            List<StrategyPnl> strategyPnls = pnLReader.Execute("pnl.csv");
            foreach (var strategy in strategyPnls)
            {
                Console.WriteLine(strategy.Strategy);
            }
        }
    }
}