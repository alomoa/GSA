using GSA.models;
using GSA.utils;

namespace Tests
{
    public class PropertiesReaderTests
    {
        PropertiesReader propertiesReader = new PropertiesReader();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PropertiesReaderAddRegion_AddsStrategyRegion()
        {
            // Arrange 
            var strategies = new List<Strategy>()
            {
               new Strategy(){StratName = "Strategy1"},
               new Strategy(){StratName = "Strategy2"},
               new Strategy(){StratName = "Strategy3"}
            };
            string[] row1 = { "Strategy1", "AP" };
            string[] row2 = { "Strategy2", "EU" };
            string[] row3 = { "Strategy3", "USA" };

            string[][] fields =
            {
                 row1,
                 row2,
                 row3
            };

            //  Act
            propertiesReader.AddRegion(strategies, fields);

            // Assert
            Assert.That(strategies[0].Region, Is.EqualTo("AP"));
            Assert.That(strategies[1].Region, Is.EqualTo("EU"));
            Assert.That(strategies[2].Region, Is.EqualTo("USA"));
        }
    }
}
