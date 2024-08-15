using PaySpace.Calculator.Application;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services;

public sealed class CalculationService : ICalculationService
{
    private readonly IHistoryService _historyService;
    private readonly IPostalCodeService _postalCodeService;
    private readonly ICalculatorSettingsService _calculatorSettingsService;
    private readonly ICalculatorStrategyFactory _calculatorStrategyFactory;

    public CalculationService(IHistoryService historyService, IPostalCodeService postalCodeService,
        ICalculatorSettingsService calculatorSettingsService, ICalculatorStrategyFactory calculatorStrategyFactory)
    {
        _historyService = historyService;
        _postalCodeService = postalCodeService;
        _calculatorSettingsService = calculatorSettingsService;
        _calculatorStrategyFactory = calculatorStrategyFactory;
    }

    public async Task<CalculateResultDto> CalculateAsync(CalculateRequest request, CancellationToken cancellationToken)
    {
        var postalCode = request.PostalCode ?? "Unknown";
        var currentPostalCodeObj = await _postalCodeService.CalculatorTypeAsync(postalCode, cancellationToken);
        var calculatorType = (CalculatorType)Enum.Parse(typeof(CalculatorType), currentPostalCodeObj.Calculator);

        var calculatorSettings = await _calculatorSettingsService.GetSettingsAsync(currentPostalCodeObj.Calculator, cancellationToken);

        var calculatorStrategy = _calculatorStrategyFactory.GetCalculatorStrategy(calculatorType);

        var calculation = await calculatorStrategy.CalculateAsync(request.Income, calculatorSettings);


        var historyDto = new CalculatorHistoryDto { PostalCode = postalCode, Income = request.Income, Tax = calculation.Tax, Calculator = currentPostalCodeObj.Calculator };
        await _historyService.AddAsync(historyDto, cancellationToken);

        return new CalculateResultDto(currentPostalCodeObj.Calculator, calculation.Tax);
    }
}

