using GSA.models;
using GSA.utils;

namespace Tests
{
    public class CapitalReaderTests
    {

        CapitalReader capitalReader = new CapitalReader();


        [Test]
        public void CapitalReaderAddFields_AddCorrectICollectionCapital()
        {
            // Arrange 
            var strategies = new List<Strategy>()
            {
               new Strategy(){StratName = "Strategy1"},
               new Strategy(){StratName = "Strategy2"},
               new Strategy(){StratName = "Strategy3"}
            };
            string[] headers = { "Strategy1", "Strategy2", "Strategy3" };
            string[] row1 = { "2010-01-01", "100", "500", "1000" };
            string[] row2 = { "2010-01-04", "150", "700", "1200" };
            string[] row3 = { "2010-01-10", "200", "850", "1300" };
            string[][] fields =
           {
                 row1,
                 row2,
                 row3
            };

            //  Act
            capitalReader.AddFields(fields, headers, strategies);

            // Assert
            Assert.That(strategies[0].Capital.Count, Is.EqualTo(3));

            Assert.That(strategies[0].Capital.ElementAt(0).Date, Is.EqualTo(new DateTime(2010, 01, 01)));
            Assert.That(strategies[0].Capital.ElementAt(1).Date, Is.EqualTo(new DateTime(2010, 01, 04)));
            Assert.That(strategies[0].Capital.ElementAt(2).Date, Is.EqualTo(new DateTime(2010, 01, 10)));

            Assert.That(strategies[0].Capital.ElementAt(0).Amount, Is.EqualTo(100M));
            Assert.That(strategies[0].Capital.ElementAt(1).Amount, Is.EqualTo(150M));
            Assert.That(strategies[0].Capital.ElementAt(2).Amount, Is.EqualTo(200M));

            Assert.That(strategies[1].Capital.ElementAt(0).Amount, Is.EqualTo(500M));
            Assert.That(strategies[1].Capital.ElementAt(1).Amount, Is.EqualTo(700M));
            Assert.That(strategies[1].Capital.ElementAt(2).Amount, Is.EqualTo(850M));

            Assert.That(strategies[2].Capital.ElementAt(0).Amount, Is.EqualTo(1000M));
            Assert.That(strategies[2].Capital.ElementAt(1).Amount, Is.EqualTo(1200M));
            Assert.That(strategies[2].Capital.ElementAt(2).Amount, Is.EqualTo(1300M));
        }
    }
}