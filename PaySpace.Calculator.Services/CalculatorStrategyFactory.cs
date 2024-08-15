using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Services.Abstractions;

namespace PaySpace.Calculator.Services;
public sealed class CalculatorStrategyFactory : ICalculatorStrategyFactory
{
    private readonly IEnumerable<ICalculatorStrategy> _strategies;

    public CalculatorStrategyFactory(IEnumerable<ICalculatorStrategy> strategies)
    {
        _strategies = strategies;
    }

    public ICalculatorStrategy GetCalculatorStrategy(CalculatorType calculatorType)
    {
        return _strategies.First(s => s.CalculatorType == calculatorType);
    }
}