namespace GSA
{
    public class PropertiesReader
    {

        public void ConvertProperties(List<Strategy> strategies)
        {
            var lines = File.ReadAllLines("properties.csv").Select(x => x.Split(",")).ToArray();
            var body = lines.Skip(1).ToArray();
            

            for (int i = 0 ; i < body.Count(); i++ )
            {
                var line = body[i];
                var strategy = strategies.Find(x => x.StratName == line[0]);

                strategy.Region = line[1];

            }
        }
    }
}
