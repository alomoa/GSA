namespace GSA
{
    public class Strategy
    {

        public Strategy()
        {
            Capital = new HashSet<Capital>();
            Pnl = new HashSet<Pnl>();
        }

        public string StratName { get; set; }
        public int StrategyId { get; set; }
        public string Region { get; set; }

        public ICollection<Pnl> Pnl { get; set; }
        public ICollection<Capital> Capital { get; set; }
    }
}