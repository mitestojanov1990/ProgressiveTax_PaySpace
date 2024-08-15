using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using NUnit.Framework;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Infrastructure.Persistence;
using PaySpace.Calculator.Services;

namespace PaySpace.Calculator.Tests;

[TestFixture]
public class CalculatorSettingsServiceTests
{
    private CalculatorContext _dbContext;
    private Mock<IMemoryCache> _mockMemoryCache;
    private CalculatorSettingsService _service;


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
        _mockMemoryCache = new Mock<IMemoryCache>();
        _service = new CalculatorSettingsService(_dbContext, _mockMemoryCache.Object);

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var calculatorSettings = new List<CalculatorSetting>
            {
                new CalculatorSetting { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 10, From = 0, To = 8350 }
            };

        _dbContext.CalculatorSettings.AddRange(calculatorSettings);
        _dbContext.SaveChanges();
    }

    [Test]
    public async Task GetSettingsAsync_Should_Return_Settings_From_Cache_If_Available()
    {
        // Arrange
        var calculatorType = "Progressive";
        var cacheKey = $"CalculatorSetting:{calculatorType}";
        var expectedSettings = new List<CalculatorSettingDto>
    {
        new CalculatorSettingDto("Progressive", "Percentage", 10, 0, 8350)
    };

        object cachedValue = expectedSettings;

        _mockMemoryCache
            .Setup(x => x.TryGetValue(cacheKey, out cachedValue))
            .Returns(true);

        // Act
        var result = await _service.GetSettingsAsync(calculatorType, CancellationToken.None);

        // Assert
        Assert.That(result.Count, Is.EqualTo(expectedSettings.Count));
        Assert.That(result[0], Is.EqualTo(expectedSettings[0]));
    }

    [Test]
    public async Task GetSettingsAsync_Should_Query_Database_If_Not_In_Cache()
    {
        // Arrange
        var calculatorType = "Progressive";
        var cancellationToken = CancellationToken.None;

        object cacheEntry = null;
        _mockMemoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

        // Act
        var result = await _service.GetSettingsAsync(calculatorType, cancellationToken);

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Calculator, Is.EqualTo("Progressive"));
    }

    [Test]
    public void GetSettingsAsync_Should_Throw_Exception_For_Invalid_CalculatorType()
    {
        // Arrange
        var invalidCalculatorType = "InvalidType";

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _service.GetSettingsAsync(invalidCalculatorType, CancellationToken.None));
    }
}