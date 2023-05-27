using GSA.Data.Entity;
using GSA.utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;


namespace Tests
{
    public class ConsoleHelpersTests
    {
        private Mock<DatabaseQuerier> _databaseQuerierMock;
        private ConsoleHelpers _consoleHelpers;

        [SetUp]
        public void SetUp()
        {
            Mock<DbContext> context = new Mock<DbContext>();
            _databaseQuerierMock = new Mock<DatabaseQuerier>(context.Object);
            _consoleHelpers = new ConsoleHelpers(_databaseQuerierMock.Object);
        }

        [Test]
        public void ProcessCommands_InvalidCommand_PrintsErrorMessage()
        {
            // Arrange
            string invalidCommand = "invalid";
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Act
            using (StringReader reader = new StringReader(invalidCommand))
            {
                Console.SetIn(reader);
                _consoleHelpers.ProcessCommands();
            }

            // Assert
            string expectedOutput = "Invalid command." + Environment.NewLine;
            Assert.That(consoleOutput.ToString(), Is.EqualTo(expectedOutput));
        }

        [Test]
        public void ProcessCommands_CapitalCommand_CallsProcessCapital()
        {
            // Arrange
            string command = "capital strategy1 strategy2";
            var consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

            // Mock the database query results
            var capital1 = new Capital { Date = new DateTime(2023, 5, 1), Amount = 1000 };
            var capital2 = new Capital { Date = new DateTime(2023, 5, 2), Amount = 2000 };
            var databaseQueryResults = new List<Strategy>
        {
            new Strategy { StratName = "strategy1", Capitals = new List<Capital> { capital1 } },
            new Strategy { StratName = "strategy2", Capitals = new List<Capital> { capital2 } }
        };
            _databaseQuerierMock.Setup(q => q.QueryCapitals(It.IsAny<string[]>())).Returns(databaseQueryResults);

            // Act
            using (StringReader reader = new StringReader(command))
            {
                Console.SetIn(reader);
                _consoleHelpers.ProcessCommands();
            }

            // Assert
            string expectedOutput =
                "strategy: strategy1, date: 2023-05-01, capital: 1000" + Environment.NewLine +
                "strategy: strategy2, date: 2023-05-02, capital: 2000" + Environment.NewLine;
            Assert.AreEqual(expectedOutput, consoleOutput.ToString());
        }
    }
}

