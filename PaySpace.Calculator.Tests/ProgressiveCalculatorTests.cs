using NUnit.Framework;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Calculators;

namespace PaySpace.Calculator.Tests;

[TestFixture]
internal sealed class ProgressiveCalculatorTests
{
    private ICalculatorStrategy _calculator;
    [SetUp]
    public void Setup()
    {
        _calculator = new ProgressiveCalculator();
    }

    [TestCase(-1, 0)]
    [TestCase(50, 5)]
    [TestCase(8350.1, 835.01)]
    [TestCase(8351, 835)]
    [TestCase(33950.1, 4674.85)]
    [TestCase(33951, 4674.85)]
    [TestCase(82251, 16749.60)]
    [TestCase(171550, 41753.32)]
    [TestCase(999999, 327681.79)]
    public async Task Calculate_Should_Return_Expected_Tax(decimal income, decimal expectedTax)
    {
        // Arrange
        var settings = new List<CalculatorSettingDto>
        {
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 10, 0, 8350),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 15, 8351, 33950),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 25, 33951, 82250),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 28, 82251, 171550),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 33, 171551, 372950),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 35, 372951, null)
        };
        // Act
        var result = await _calculator.CalculateAsync(income, settings);

        // Assert
        Assert.That(result.Tax, Is.EqualTo(expectedTax));
    }


    /// <summary>
    /// For income up to 8,350:
    ///    Tax = Income* 10%
    /// For income 8,351 to 33,950:
    ///     Tax = 835 + (Income - 8,350) * 15%
    /// For income 33,951 to 82,250:
    ///     Tax = 4,674.86 + (Income - 33,950) * 25%
    /// For income 82,251 to 171,550:
    ///     Tax = 16,749.60 + (Income - 82,250) * 28%
    /// For income 171,551 to 372,950:
    ///     Tax = 41,753.32 + (Income - 171,550) * 33%
    /// For income 372,951 and above:
    ///     Tax = 108,214.99 + (Income - 372,950) * 35%
    /// </summary>
    /// <param name="income"></param>
    /// <param name="expectedTax"></param>
    /// <returns></returns>
    [TestCase(0.0, 0.0)] // no tax
    [TestCase(8350.0, 835)] // edge case
    [TestCase(8350.1, 835.01)] // edge case
    [TestCase(33950.0, 4674.85)]
    [TestCase(82250.0, 16749.60)]
    [TestCase(171550.0, 41753.32)]
    [TestCase(171551.0, 41753.32)]
    [TestCase(372950.0, 108214.99)]
    [TestCase(372951.0, 108214.99)]
    [TestCase(999999, 327681.79)]
    [TestCase(1000000, 327682.14)]
    public async Task Calculate_Should_Return_EdgeCases_Expected_Tax(decimal income, decimal expectedTax)
    {
        // Arrange
        var settings = new List<CalculatorSettingDto>
        {
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 10, 0, 8350),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 15, 8351, 33950),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 25, 33951, 82250),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 28, 82251, 171550),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 33, 171551, 372950),
            new CalculatorSettingDto(CalculatorType.Progressive.ToString(), RateType.Percentage.ToString(), 35, 372951, null)
        };
        // Act
        var result = await _calculator.CalculateAsync(income, settings);

        // Assert
        Assert.That(result.Tax, Is.EqualTo(expectedTax));
    }
}