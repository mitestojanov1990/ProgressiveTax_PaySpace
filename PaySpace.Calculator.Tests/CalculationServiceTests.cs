using NUnit.Framework;
using Moq;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Application;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Tests;

[TestFixture]
public class CalculationServiceTests
{
    private Mock<IHistoryService> _mockHistoryService;
    private Mock<IPostalCodeService> _mockPostalCodeService;
    private Mock<ICalculatorSettingsService> _mockCalculatorSettingsService;
    private Mock<ICalculatorStrategyFactory> _mockCalculatorStrategyFactory;
    private Mock<ICalculatorStrategy> _mockCalculatorStrategy;
    private CalculationService _calculationService;

    [SetUp]
    public void SetUp()
    {
        _mockHistoryService = new Mock<IHistoryService>();
        _mockPostalCodeService = new Mock<IPostalCodeService>();
        _mockCalculatorSettingsService = new Mock<ICalculatorSettingsService>();
        _mockCalculatorStrategyFactory = new Mock<ICalculatorStrategyFactory>();
        _mockCalculatorStrategy = new Mock<ICalculatorStrategy>();

        _calculationService = new CalculationService(
            _mockHistoryService.Object,
            _mockPostalCodeService.Object,
            _mockCalculatorSettingsService.Object,
            _mockCalculatorStrategyFactory.Object
        );
    }

    [Test]
    public async Task CalculateAsync_Should_Return_Expected_Tax_For_Valid_PostalCode()
    {
        // Arrange
        var request = new CalculateRequest("7441", 100000);
        var cancellationToken = CancellationToken.None;

        var postalCodeResponse = new PostalCodeDto("7441", "Progressive");
        _mockPostalCodeService.Setup(x => x.CalculatorTypeAsync("7441", cancellationToken))
            .ReturnsAsync(postalCodeResponse);

        var settings = new List<CalculatorSettingDto>
        {
            new CalculatorSettingDto("Progressive", "Percentage", 10, 0, 8350)
        };
        _mockCalculatorSettingsService.Setup(x => x.GetSettingsAsync("Progressive", cancellationToken))
            .ReturnsAsync(settings);

        var calculationResult = new CalculateResultDto("Progressive", 10000);
        _mockCalculatorStrategy.Setup(x => x.CalculateAsync(request.Income, settings))
            .ReturnsAsync(calculationResult);

        _mockCalculatorStrategyFactory.Setup(x => x.GetCalculatorStrategy(CalculatorType.Progressive))
            .Returns(_mockCalculatorStrategy.Object);

        // Act
        var result = await _calculationService.CalculateAsync(request, cancellationToken);

        // Assert
        Assert.That(result.Calculator, Is.EqualTo("Progressive"));
        Assert.That(result.Tax, Is.EqualTo(10000));

        _mockHistoryService.Verify(x => x.AddAsync(It.Is<CalculatorHistoryDto>(h =>
            h.PostalCode == "7441" &&
            h.Income == 100000 &&
            h.Tax == 10000 &&
            h.Calculator == "Progressive"), cancellationToken), Times.Once);
    }

    [Test]
    public void CalculateAsync_Should_Throw_Exception_For_Invalid_PostalCode()
    {
        // Arrange
        var request = new CalculateRequest("Invalid", 100000);
        var cancellationToken = CancellationToken.None;

        _mockPostalCodeService.Setup(x => x.CalculatorTypeAsync("Invalid", cancellationToken))
            .ThrowsAsync(new ArgumentException("Postal code 'Invalid' not found."));

        // Act & Assert
        var ex = Assert.ThrowsAsync<ArgumentException>(() =>
            _calculationService.CalculateAsync(request, cancellationToken));

        Assert.That(ex.Message, Is.EqualTo("Postal code 'Invalid' not found."));
    }

    [Test]
    public async Task CalculateAsync_Should_Call_AddAsync_On_HistoryService()
    {
        // Arrange
        var request = new CalculateRequest("7441", 100000);
        var cancellationToken = CancellationToken.None;

        var postalCodeResponse = new PostalCodeDto("7441", "Progressive");
        _mockPostalCodeService.Setup(x => x.CalculatorTypeAsync("7441", cancellationToken))
            .ReturnsAsync(postalCodeResponse);

        var settings = new List<CalculatorSettingDto>
        {
            new CalculatorSettingDto("Progressive", "Percentage", 10, 0, 8350)
        };
        _mockCalculatorSettingsService.Setup(x => x.GetSettingsAsync("Progressive", cancellationToken))
            .ReturnsAsync(settings);

        var calculationResult = new CalculateResultDto("Progressive", 10000);
        _mockCalculatorStrategy.Setup(x => x.CalculateAsync(request.Income, settings))
            .ReturnsAsync(calculationResult);

        _mockCalculatorStrategyFactory.Setup(x => x.GetCalculatorStrategy(CalculatorType.Progressive))
            .Returns(_mockCalculatorStrategy.Object);

        // Act
        await _calculationService.CalculateAsync(request, cancellationToken);

        // Assert
        _mockHistoryService.Verify(x => x.AddAsync(It.IsAny<CalculatorHistoryDto>(), cancellationToken), Times.Once);
    }
}
