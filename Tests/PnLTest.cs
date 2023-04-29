using gsa;

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
        public void ShouldReturnAnListOfStrategyPnlGivenAValidPath() {
            //Arrange & Act
            List<StrategyPnl> strategyPnls = pnLReader.Read("pnl.csv");

            //Assert
            Assert.That(strategyPnls.Count, Is.EqualTo(15));
        }

        [Test]
        public void ShouldReturnAnEmptyListIfGivenAnInvalidPath() {
            
        }

        [Test]
        public void ShouldReturnAListOfHeadersFromTheStartOfACSVFile()
        {

        }


        
    }
}