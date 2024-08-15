using PaySpace.Calculator.Application;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Application.DTO;

namespace PaySpace.Calculator.Services.Calculators;

internal sealed class FlatValueCalculator : ICalculatorStrategy
{
    public CalculatorType CalculatorType => CalculatorType.FlatValue;

    public Task<CalculateResultDto> CalculateAsync(decimal income, List<CalculatorSettingDto> settings)
    {
        decimal tax;
        if (income < 200000)
        {
            tax = income * 0.05M;
        }
        else
        {
            tax = 10000;
        }

        return Task.FromResult(new CalculateResultDto(CalculatorType.ToString(), tax));
    }
}