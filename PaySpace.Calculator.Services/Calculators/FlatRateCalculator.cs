using PaySpace.Calculator.Application;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Application.DTO;

namespace PaySpace.Calculator.Services.Calculators;

internal sealed class FlatRateCalculator : ICalculatorStrategy
{
    public CalculatorType CalculatorType => CalculatorType.FlatRate;

    public Task<CalculateResultDto> CalculateAsync(decimal income, List<CalculatorSettingDto> settings)
    {
        var tax = income * 0.175M;
        return Task.FromResult(new CalculateResultDto(CalculatorType.ToString(), tax));
    }
}