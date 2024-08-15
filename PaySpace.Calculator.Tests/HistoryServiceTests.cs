using NUnit.Framework;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Infrastructure.Persistence;
using PaySpace.Calculator.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PaySpace.Calculator.Application;

namespace PaySpace.Calculator.Tests
{
    [TestFixture]
    public class HistoryServiceTests
    {
        private CalculatorContext _dbContext;
        private HistoryService _historyService;

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<CalculatorContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new CalculatorContext(options);
            _historyService = new HistoryService(_dbContext);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var calculatorHistories = new List<CalculatorHistory>
            {
                new CalculatorHistory("1000", 50000, 7500, CalculatorType.Progressive) { Timestamp = System.DateTime.UtcNow.AddDays(-2) },
                new CalculatorHistory("7000", 60000, 10500, CalculatorType.FlatRate) { Timestamp = System.DateTime.UtcNow.AddDays(-1) }
            };

            _dbContext.CalculatorHistories.AddRange(calculatorHistories);
            _dbContext.SaveChanges();
        }

        [Test]
        public async Task AddAsync_Should_Add_History_To_Database()
        {
            // Arrange
            var newHistory = new CalculatorHistoryDto { PostalCode = "7441", Income = 100000, Tax = 15000, Calculator = "Progressive", Timestamp = DateTime.Now };
            var cancellationToken = CancellationToken.None;

            // Act
            await _historyService.AddAsync(newHistory, cancellationToken);

            // Assert
            var addedHistory = await _dbContext.CalculatorHistories.FirstOrDefaultAsync(h => h.PostalCode == "7441", cancellationToken);
            Assert.IsNotNull(addedHistory);
            Assert.That(addedHistory.PostalCode, Is.EqualTo(newHistory.PostalCode));
            Assert.That(addedHistory.Income, Is.EqualTo(newHistory.Income));
            Assert.That(addedHistory.Tax, Is.EqualTo(newHistory.Tax));
            Assert.That(addedHistory.Calculator, Is.EqualTo(CalculatorType.Progressive));
        }

        [Test]
        public async Task GetHistoryAsync_Should_Return_History_From_Database_In_Descending_Order()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _historyService.GetHistoryAsync(cancellationToken);

            // Assert
            var resultList = result.ToList();
            Assert.That(resultList.Count, Is.EqualTo(2));
            Assert.That(resultList[0].PostalCode, Is.EqualTo("7000"));
            Assert.That(resultList[1].PostalCode, Is.EqualTo("1000"));
        }
    }
}
