using PaySpace.Calculator.Application.DTO;

namespace PaySpace.Calculator.Application.Abstractions;

public interface ICalculatorSettingsService: IScopedService
{
    Task<List<CalculatorSettingDto>> GetSettingsAsync(string calculatorType, CancellationToken cancellationToken);
}