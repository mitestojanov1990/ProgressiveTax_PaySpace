using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Domain.Enum;

namespace PaySpace.Calculator.Services.Abstractions;
public interface ICalculatorStrategyFactory: IScopedService
{
    ICalculatorStrategy GetCalculatorStrategy(CalculatorType calculatorType);
}