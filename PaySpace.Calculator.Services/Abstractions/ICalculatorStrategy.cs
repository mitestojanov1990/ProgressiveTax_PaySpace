using PaySpace.Calculator.Application;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Application.Abstractions;

namespace PaySpace.Calculator.Services.Abstractions;

public interface ICalculatorStrategy: IScopedService
{
    Task<CalculateResultDto> CalculateAsync(decimal income, List<CalculatorSettingDto> settings);
    CalculatorType CalculatorType { get; }
}