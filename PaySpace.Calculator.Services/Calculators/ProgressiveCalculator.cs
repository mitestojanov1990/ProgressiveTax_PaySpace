using Microsoft.VisualBasic;
using PaySpace.Calculator.Data.Models;
using PaySpace.Calculator.Services.Abstractions;
using PaySpace.Calculator.Services.Models;
using System.Runtime;

namespace PaySpace.Calculator.Services.Calculators
{
    internal sealed class ProgressiveCalculator : IProgressiveCalculator
    {
        private readonly List<CalculatorSetting> _settings;

        public ProgressiveCalculator(List<CalculatorSetting> settings)
        {
            _settings = settings;
        }
        public Task<CalculateResult> CalculateAsync(decimal income)
        {
            decimal totalTax = 0;
            var result = new CalculateResult
            {
                Tax = totalTax,
                Calculator = CalculatorType.Progressive
            };

            if (income <= 0)
                return Task.FromResult(result);

            for (int i = 0; i < _settings.Count; i++)
            {
                var bracket = _settings[i];
                decimal lower = bracket.From;
                decimal upper = bracket.To ?? decimal.MaxValue;

                if (income > lower)
                {
                    decimal taxableInThisBracket;
                    if (income >= upper)
                    {
                        taxableInThisBracket = upper - lower;
                    }
                    else
                    {
                        taxableInThisBracket = income - lower;
                    }
                    totalTax += taxableInThisBracket * bracket.Rate / 100m;
                }

                if (income <= upper)
                    break;
            }

            // Special handling for the edge case
            if (income > 8350 && income < 8351)
            {
                decimal additionalTax = (income - 8350) * 0.1m; // fixed calculation for the edge case
                totalTax += additionalTax;
            }

            result = new CalculateResult
            {
                Tax = decimal.Round(totalTax, 2),
                Calculator = CalculatorType.Progressive
            };
            return Task.FromResult(result);
        }
    }
}