using NUnit.Framework;
using Moq;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Tests;

[TestFixture]
public class CalculatorStrategyFactoryTests
{
    private CalculatorStrategyFactory _factory;
    private Mock<ICalculatorStrategy> _progressiveStrategyMock;
    private Mock<ICalculatorStrategy> _flatRateStrategyMock;
    private Mock<ICalculatorStrategy> _flatValueStrategyMock;

    [SetUp]
    public void SetUp()
    {
        _progressiveStrategyMock = new Mock<ICalculatorStrategy>();
        _flatRateStrategyMock = new Mock<ICalculatorStrategy>();
        _flatValueStrategyMock = new Mock<ICalculatorStrategy>();

        _progressiveStrategyMock.SetupGet(s => s.CalculatorType).Returns(CalculatorType.Progressive);
        _flatRateStrategyMock.SetupGet(s => s.CalculatorType).Returns(CalculatorType.FlatRate);
        _flatValueStrategyMock.SetupGet(s => s.CalculatorType).Returns(CalculatorType.FlatValue);

        var strategies = new List<ICalculatorStrategy>
        {
            _progressiveStrategyMock.Object,
            _flatRateStrategyMock.Object,
            _flatValueStrategyMock.Object
        };

        _factory = new CalculatorStrategyFactory(strategies);
    }

    [Test]
    public void GetCalculatorStrategy_Should_Return_ProgressiveStrategy_When_CalculatorType_Is_Progressive()
    {
        // Act
        var result = _factory.GetCalculatorStrategy(CalculatorType.Progressive);

        // Assert
        Assert.That(result, Is.EqualTo(_progressiveStrategyMock.Object));
    }

    [Test]
    public void GetCalculatorStrategy_Should_Return_FlatRateStrategy_When_CalculatorType_Is_FlatRate()
    {
        // Act
        var result = _factory.GetCalculatorStrategy(CalculatorType.FlatRate);

        // Assert
        Assert.That(result, Is.EqualTo(_flatRateStrategyMock.Object));
    }

    [Test]
    public void GetCalculatorStrategy_Should_Return_FlatValueStrategy_When_CalculatorType_Is_FlatValue()
    {
        // Act
        var result = _factory.GetCalculatorStrategy(CalculatorType.FlatValue);

        // Assert
        Assert.That(result, Is.EqualTo(_flatValueStrategyMock.Object));
    }

    [Test]
    public void GetCalculatorStrategy_Should_Throw_Exception_When_CalculatorType_Is_Unknown()
    {
        // Arrange
        var unknownCalculatorType = (CalculatorType)999;

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => _factory.GetCalculatorStrategy(unknownCalculatorType));
        Assert.That(ex.Message, Is.EqualTo("Sequence contains no matching element"));
    }
}
