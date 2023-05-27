using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using GSA.models;
using GSA.utils;
using Moq;

namespace Tests
{
    
    public class StrategyReaderTest
    {
        GSA.utils.StrategyReader strategyReader;

        [Test]
        public void ShouldWork()
        {
            //Arrange
            string[] capitals = {
                "Date,Strategy1,Strategy2,Strategy3",
                "2010-01-01,120500000,118000000,98000000",
            };
            string[] pnls =
            {
                "Date,Strategy1,Strategy2,Strategy3",
                "2010-01-01,95045,501273,429834",
            };

            string[] properties =
            {
                "Strategy,Region",
                "Strategy1,AP",
                "Strategy2,EU",
                "Strategy3,EU"
            };

            Mock<IMyFileReader> mockFileReader = new Mock<IMyFileReader>();
            mockFileReader.Setup(x => x.ReadAllLines("files/pnl.csv")).Returns(pnls);
            mockFileReader.Setup(x => x.ReadAllLines("files/capital.csv")).Returns(capitals);
            mockFileReader.Setup(x => x.ReadAllLines("files/properties.csv")).Returns(properties);

            strategyReader = new StrategyReader(mockFileReader.Object);

            //Act
            var result = strategyReader.Execute();
            var resultStrategyOne = result.Where(x => x.StratName == "Strategy1").First();


            var resultStrategyTwo = result.Where(x => x.StratName == "Strategy2").First();
            var resultStrategyThree = result.Where(x => x.StratName == "Strategy3").First();

            //Assert
            Assert.That(resultStrategyOne.StratName, Is.EqualTo("Strategy1"));
            Assert.That(resultStrategyOne.Capital.First().Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(resultStrategyOne.Capital.First().Amount, Is.EqualTo(120500000));
            Assert.That(resultStrategyOne.Pnl.First().Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(resultStrategyOne.Pnl.First().Amount, Is.EqualTo(95045));

            Assert.That(resultStrategyTwo.StratName, Is.EqualTo("Strategy2"));
            Assert.That(resultStrategyTwo.Capital.First().Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(resultStrategyTwo.Capital.First().Amount, Is.EqualTo(118000000));
            Assert.That(resultStrategyTwo.Pnl.First().Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(resultStrategyTwo.Pnl.First().Amount, Is.EqualTo(501273));

            Assert.That(resultStrategyThree.StratName, Is.EqualTo("Strategy3"));
            Assert.That(resultStrategyThree.Capital.First().Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(resultStrategyThree.Capital.First().Amount, Is.EqualTo(98000000));
            Assert.That(resultStrategyThree.Pnl.First().Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(resultStrategyThree.Pnl.First().Amount, Is.EqualTo(429834));
        }

        [Test]
        public void ShouldReturnAListOfPnls()
        {
            //Arrange
            string[] pnls =
            {
                "Date,Strategy1,Strategy2,Strategy3",
                "2010-01-01,95045,501273,429834",
                "2010-01-04,-140135,369071,153109",
                "2010-01-05,-106757,3868,483559",
                "2010-01-06,219858,96525,-344514"
            };


            Mock<IMyFileReader> mockFileReader = new Mock<IMyFileReader>();
            mockFileReader.Setup(x => x.ReadAllLines("files/pnl.csv")).Returns(pnls);

            strategyReader = new StrategyReader(mockFileReader.Object);

            //Act
            var result = strategyReader.ReadPnls();

            //Assert
            Assert.That(result.ElementAt(0).Pnl.ElementAt(0).Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(result.ElementAt(0).Pnl.ElementAt(1).Date, Is.EqualTo(DateTime.Parse("2010-01-04")));
            Assert.That(result.ElementAt(0).Pnl.ElementAt(2).Date, Is.EqualTo(DateTime.Parse("2010-01-05")));
            Assert.That(result.ElementAt(0).Pnl.ElementAt(3).Date, Is.EqualTo(DateTime.Parse("2010-01-06")));
           
            Assert.That(result.ElementAt(0).Pnl.ElementAt(0).Amount, Is.EqualTo(95045));
            Assert.That(result.ElementAt(0).Pnl.ElementAt(1).Amount, Is.EqualTo(-140135));
            Assert.That(result.ElementAt(0).Pnl.ElementAt(2).Amount, Is.EqualTo(-106757));
            Assert.That(result.ElementAt(0).Pnl.ElementAt(3).Amount, Is.EqualTo(219858));

            Assert.That(result.ElementAt(1).Pnl.ElementAt(0).Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(result.ElementAt(1).Pnl.ElementAt(1).Date, Is.EqualTo(DateTime.Parse("2010-01-04")));
            Assert.That(result.ElementAt(1).Pnl.ElementAt(2).Date, Is.EqualTo(DateTime.Parse("2010-01-05")));
            Assert.That(result.ElementAt(1).Pnl.ElementAt(3).Date, Is.EqualTo(DateTime.Parse("2010-01-06")));

            Assert.That(result.ElementAt(1).Pnl.ElementAt(0).Amount, Is.EqualTo(501273));
            Assert.That(result.ElementAt(1).Pnl.ElementAt(1).Amount, Is.EqualTo(369071));
            Assert.That(result.ElementAt(1).Pnl.ElementAt(2).Amount, Is.EqualTo(3868));
            Assert.That(result.ElementAt(1).Pnl.ElementAt(3).Amount, Is.EqualTo(96525));

            Assert.That(result.ElementAt(2).Pnl.ElementAt(0).Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(result.ElementAt(2).Pnl.ElementAt(1).Date, Is.EqualTo(DateTime.Parse("2010-01-04")));
            Assert.That(result.ElementAt(2).Pnl.ElementAt(2).Date, Is.EqualTo(DateTime.Parse("2010-01-05")));
            Assert.That(result.ElementAt(2).Pnl.ElementAt(3).Date, Is.EqualTo(DateTime.Parse("2010-01-06")));

            Assert.That(result.ElementAt(2).Pnl.ElementAt(0).Amount, Is.EqualTo(429834));
            Assert.That(result.ElementAt(2).Pnl.ElementAt(1).Amount, Is.EqualTo(153109));
            Assert.That(result.ElementAt(2).Pnl.ElementAt(2).Amount, Is.EqualTo(483559));
            Assert.That(result.ElementAt(2).Pnl.ElementAt(3).Amount, Is.EqualTo(-344514));

        }

        [Test]
        public void ShouldReturnAListOfCapitals()
        {
            //Arrange
            string[] capitals =
            {
                "Date,Strategy1,Strategy2,Strategy3",
                "2010-01-01,120500000,118000000,98000000",
                "2010-02-01,199500000,124000000,105000000",
                "2010-03-01,16500000,123500000,53500000",
                "2010-04-01,136000000,163000000,58000000"
            };


            Mock<IMyFileReader> mockFileReader = new Mock<IMyFileReader>();
            mockFileReader.Setup(x => x.ReadAllLines("files/capital.csv")).Returns(capitals);

            strategyReader = new StrategyReader(mockFileReader.Object);

            //Act
            var result = strategyReader.ReadCapitals();

            //Assert
            Assert.That(result.ElementAt(0).Capital.ElementAt(0).Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(result.ElementAt(0).Capital.ElementAt(1).Date, Is.EqualTo(DateTime.Parse("2010-02-01")));
            Assert.That(result.ElementAt(0).Capital.ElementAt(2).Date, Is.EqualTo(DateTime.Parse("2010-03-01")));
            Assert.That(result.ElementAt(0).Capital.ElementAt(3).Date, Is.EqualTo(DateTime.Parse("2010-04-01")));
                                            
            Assert.That(result.ElementAt(0).Capital.ElementAt(0).Amount, Is.EqualTo(120500000));
            Assert.That(result.ElementAt(0).Capital.ElementAt(1).Amount, Is.EqualTo(199500000));
            Assert.That(result.ElementAt(0).Capital.ElementAt(2).Amount, Is.EqualTo(16500000));
            Assert.That(result.ElementAt(0).Capital.ElementAt(3).Amount, Is.EqualTo(136000000));
                                            
            Assert.That(result.ElementAt(1).Capital.ElementAt(0).Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(result.ElementAt(1).Capital.ElementAt(1).Date, Is.EqualTo(DateTime.Parse("2010-02-01")));
            Assert.That(result.ElementAt(1).Capital.ElementAt(2).Date, Is.EqualTo(DateTime.Parse("2010-03-01")));
            Assert.That(result.ElementAt(1).Capital.ElementAt(3).Date, Is.EqualTo(DateTime.Parse("2010-04-01")));
                                            
            Assert.That(result.ElementAt(1).Capital.ElementAt(0).Amount, Is.EqualTo(118000000));
            Assert.That(result.ElementAt(1).Capital.ElementAt(1).Amount, Is.EqualTo(124000000));
            Assert.That(result.ElementAt(1).Capital.ElementAt(2).Amount, Is.EqualTo(123500000));
            Assert.That(result.ElementAt(1).Capital.ElementAt(3).Amount, Is.EqualTo(163000000));
                                            
            Assert.That(result.ElementAt(2).Capital.ElementAt(0).Date, Is.EqualTo(DateTime.Parse("2010-01-01")));
            Assert.That(result.ElementAt(2).Capital.ElementAt(1).Date, Is.EqualTo(DateTime.Parse("2010-02-01")));
            Assert.That(result.ElementAt(2).Capital.ElementAt(2).Date, Is.EqualTo(DateTime.Parse("2010-03-01")));
            Assert.That(result.ElementAt(2).Capital.ElementAt(3).Date, Is.EqualTo(DateTime.Parse("2010-04-01")));
                                            
            Assert.That(result.ElementAt(2).Capital.ElementAt(0).Amount, Is.EqualTo(98000000));
            Assert.That(result.ElementAt(2).Capital.ElementAt(1).Amount, Is.EqualTo(105000000));
            Assert.That(result.ElementAt(2).Capital.ElementAt(2).Amount, Is.EqualTo(53500000));
            Assert.That(result.ElementAt(2).Capital.ElementAt(3).Amount, Is.EqualTo(58000000));
        }

        [Test]
        public void ShouldThrowExceptionIfFirstColumnHeaderIsNotDate()
        {
            //Arrange
            string[] lines =
            {
                "Dote,Strategy1,Strategy2,Strategy3",
                "2010-01-01,95045,501273,429834",
            };


            Mock<IMyFileReader> mockFileReader = new Mock<IMyFileReader>();

            strategyReader = new StrategyReader(mockFileReader.Object);

            //Act & Assert

            Assert.Throws<Exception>(() => strategyReader.GetStrategyNames(lines));
        }

        [Test]
        public void ShouldThrowExceptionIfPropertiesHeadersAreIncorrect()
        {
            //Arrange
            string[] lines =
            {
                "Strame,Ren",
                "Strategy1,AP"
            };

            Mock<IMyFileReader> mockFileReader = new Mock<IMyFileReader>();
            strategyReader = new StrategyReader(mockFileReader.Object);

            //Act & Assert
            Assert.Throws<Exception>(() => strategyReader.ParseProperties(lines));
        }
    }
}
