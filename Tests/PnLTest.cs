using gsa;
using NuGet.Frameworks;

namespace Tests
{
    public class Tests
    {

        PnLReader pnLReader = new PnLReader();

        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void ShouldReturnAnListOfStrategyPnl() {
            //Arrange
            var headers = "Date,Strategy1,Strategy2,Strategy3,Strategy4";
            var row1 = "2010-01-01,95045,501273,429834";
            var row2 = "2010-01-04,-140135,369071,153109";
            string[] lines = { headers, row1, row2 };

            //Act
            List<Strategy> strategyPnls = pnLReader.Read(lines);

            //Assert
            Assert.That(strategyPnls.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldReturnAListOfStrategyPnLs() {

            //Arrange
            string[] headers = {"Strategy1", "Strategy2", "Strategy3", "Strategy4"};
            var strategies = new List<Strategy>();
            
            //Act
            pnLReader.SetupStrategyPnL(headers, strategies);

            //Assert
            Assert.That(strategies.Count, Is.EqualTo(4));
        }

        [Test]
        public void ShouldAddFieldsToStrategies()
        {
            //Arrange
            string[] headers = { "Strategy1", "Strategy2", "Strategy3", "Strategy4" };
            string[] row1 = { "2010-01-01", "95045", "501273", "429834" };
            string[] row2 = { "2010-01-04","-140135","369071","153109" };
            string[][] fields =
            {
                 row1,
                 row2
            };
            var strategies = new List<Strategy>();
            pnLReader.SetupStrategyPnL(headers, strategies);

            //Act
            pnLReader.AddFields(fields, strategies);

            //Assert
            Assert.That(strategies[0].Pnls.Count, Is.EqualTo(2));
        }
    }
}