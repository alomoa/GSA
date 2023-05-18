using GSA.models;

namespace GSA.utils
{
    public class PropertiesReader
    {

        public void ConvertProperties(List<Strategy> strategies)
        {
            var lines = Read("files/properties.csv");
            AddRegion(strategies, lines);
        }

        private string[][] Read(string filePath)
        {
            return File.ReadAllLines(filePath)
                .Select(x => x.Split(","))
                .Skip(1)
                .ToArray();
        }

        public void AddRegion(List<Strategy> strategies, string[][] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var strategy = strategies.Find(x => x.StratName == line[0]);
                if (strategy != null)
                {
                    strategy.Region = line[1];
                }
            }
        }

    }
}
