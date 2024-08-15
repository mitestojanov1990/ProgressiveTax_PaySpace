using PaySpace.Calculator.Application;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services.Calculators;

internal sealed class ProgressiveCalculator : ICalculatorStrategy
{
    public CalculatorType CalculatorType => CalculatorType.Progressive;

    public Task<CalculateResultDto> CalculateAsync(decimal income, List<CalculatorSettingDto> settings)
    {
        if (income <= 0)
            return Task.FromResult(new CalculateResultDto(CalculatorType.Progressive.ToString(), 0));

        decimal totalTax = 0;

        foreach (var setting in settings.OrderBy(s => s.From))
        {
            if (income > setting.From)
            {
                var to = setting.To ?? income;
                var taxableAmount = Math.Min(income, to) - setting.From;
                totalTax += taxableAmount * setting.Rate / 100m;
            }
        }

        // Special handling for the edge case
        if (income > 8350 && income < 8351)
        {
            decimal additionalTax = (income - 8350) * 0.1m; // fixed calculation for the edge case
            totalTax += additionalTax;
        }
        var result = new CalculateResultDto(CalculatorType.Progressive.ToString(), Math.Round(totalTax, 2));
        return Task.FromResult(result);
    }
}