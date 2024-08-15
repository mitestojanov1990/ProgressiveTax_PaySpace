using NUnit.Framework;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests;

[TestFixture]
internal sealed class FlatValueCalculatorTests
{
    private ICalculatorStrategy _calculator;
    [SetUp]
    public void Setup()
    {
        _calculator = new FlatValueCalculator();
    }

    [TestCase(199999, 9999.95)]
    [TestCase(100, 5)]
    [TestCase(200000, 10000)]
    [TestCase(6000000, 10000)]
    public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
    {
        // Arrange
        var settings = new List<CalculatorSettingDto>
        {
            new CalculatorSettingDto(CalculatorType.FlatValue.ToString(), RateType.Percentage.ToString(), 5, 0, 199999),
            new CalculatorSettingDto(CalculatorType.FlatValue.ToString(), RateType.Amount.ToString(), 10000, 200000, null),
        };

        // Act
        var result = await _calculator.CalculateAsync(income, settings);

        // Assert
        Assert.That(result.Tax, Is.EqualTo(expectedTax));
    }
}