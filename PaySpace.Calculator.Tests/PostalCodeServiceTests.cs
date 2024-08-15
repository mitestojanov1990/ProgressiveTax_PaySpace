using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PaySpace.Calculator.Application;
using PaySpace.Calculator.Domain;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Infrastructure.Persistence;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Tests;
[TestFixture]
public class PostalCodeServiceTests
{
    private FakeMemoryCache _fakeMemoryCache;
    private CalculatorContext _dbContext;

    private PostalCodeService _service;

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

        SeedDatabase();

        _fakeMemoryCache = new FakeMemoryCache();
        _service = new PostalCodeService(_dbContext, _fakeMemoryCache);
    }

    [Test]
    public async Task GetPostalCodesAsync_Should_Return_Cached_Value()
    {
        // Arrange
        var expectedPostalCodes = new List<PostalCodeDto>
            {
                new PostalCodeDto("7441","Progressive"),
                new PostalCodeDto("A100", "FlatValue")
            };

        _fakeMemoryCache.CreateEntry("PostalCodes").Value = expectedPostalCodes;

        // Act
        var result = await _service.GetPostalCodesAsync(CancellationToken.None);

        // Assert
        Assert.That(result.Count, Is.EqualTo(expectedPostalCodes.Count));
        Assert.That(result[0].Code, Is.EqualTo(expectedPostalCodes[0].Code));
    }

    [Test]
    public async Task GetPostalCodesAsync_Should_Store_Value_If_Not_Cached()
    {
        // Act
        var result = await _service.GetPostalCodesAsync(CancellationToken.None);

        // Assert
        Assert.IsTrue(_fakeMemoryCache.TryGetValue("PostalCodes", out var cachedValue));
        Assert.That(cachedValue, Is.EqualTo(result));
    }

    [Test]
    public async Task CalculatorTypeAsync_Should_Return_CalculatorType_For_Valid_Code()
    {
        // Act
        var result = await _service.CalculatorTypeAsync("7441", CancellationToken.None);

        // Assert
        var postalCodeResponse = new PostalCodeDto("7441", CalculatorType.Progressive.ToString());
        Assert.That(result, Is.EqualTo(postalCodeResponse));
    }

    [Test]
    public void CalculatorTypeAsync_Should_Throw_Exception_For_Invalid_Code()
    {
        // Act & Assert
        var ex = Assert.ThrowsAsync<CalculatorException>(() => _service.CalculatorTypeAsync("InvalidCode", CancellationToken.None));
        Assert.That(ex.Message, Is.EqualTo("Invalid Postal code. Calculator not found"));
    }

    [Test]
    public async Task CalculatorExistsForCode_Should_Return_True_If_Code_Exists()
    {
        // Act
        var result = await _service.CalculatorExistsForCode("7441");

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task CalculatorExistsForCode_Should_Return_False_If_Code_Does_Not_Exist()
    {
        // Act
        var result = await _service.CalculatorExistsForCode("InvalidCode");

        // Assert
        Assert.IsFalse(result);
    }

    private void SeedDatabase()
    {
        var postalCodes = new List<PostalCode>
            {
                new PostalCode { Code = "7441", Calculator = CalculatorType.Progressive },
                new PostalCode { Code = "A100", Calculator = CalculatorType.FlatValue },
                new PostalCode { Code = "7000", Calculator = CalculatorType.FlatRate },
                new PostalCode { Code = "1000", Calculator = CalculatorType.Progressive }
            };

        _dbContext.PostalCodes.AddRange(postalCodes);
        _dbContext.SaveChanges();
    }
}