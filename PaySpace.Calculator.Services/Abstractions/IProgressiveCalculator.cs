using PaySpace.Calculator.Application;
using PaySpace.Calculator.Domain;

namespace PaySpace.Calculator.Services.Abstractions;

public interface IProgressiveCalculator
{
    Task<CalculateResultDto> CalculateAsync(decimal income, CalculatorSetting setting);
}