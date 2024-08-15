using Moq;

using NUnit.Framework;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests;

[TestFixture]
internal sealed class FlatRateCalculatorTests
{
    private ICalculatorStrategy _calculator;
    [SetUp]
    public void Setup()
    {
        _calculator = new FlatRateCalculator();
    }

    [TestCase(999999, 174999.825)]
    [TestCase(1000, 175)]
    [TestCase(5, 0.875)]
    public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
    {
        // Arrange
        var settings = new List<CalculatorSettingDto>
        {
            new CalculatorSettingDto(CalculatorType.FlatRate.ToString(), RateType.Percentage.ToString(), 17.5M, 0, null),
        };

        // Act
        var result = await _calculator.CalculateAsync(income, settings);

        // Assert
        Assert.That(result.Tax, Is.EqualTo(expectedTax));
    }
}
