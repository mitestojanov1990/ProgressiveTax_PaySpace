using PaySpace.Calculator.Domain.Enum;

namespace PaySpace.Calculator.Domain;

public sealed class PostalCode: BaseEntity<long>
{
    public string Code { get; set; } = default!;

    public CalculatorType Calculator { get; set; }
}