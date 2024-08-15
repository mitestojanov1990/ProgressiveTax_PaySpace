using PaySpace.Calculator.Domain.Enum;

namespace PaySpace.Calculator.Domain;

public sealed class CalculatorSetting : BaseEntity<long>
{
    public CalculatorType Calculator { get; set; }

    public RateType RateType { get; set; }

    public decimal Rate { get; set; }

    public decimal From { get; set; }

    public decimal? To { get; set; }
}