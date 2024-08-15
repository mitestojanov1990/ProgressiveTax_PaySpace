using PaySpace.Calculator.Domain.Enum;

namespace PaySpace.Calculator.Domain;

public sealed class CalculatorHistory : BaseEntity<long>
{
    public string PostalCode { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    public decimal Income { get; set; }

    public decimal Tax { get; set; }

    public CalculatorType Calculator { get; set; }

    public CalculatorHistory(string postalCode, decimal income, decimal tax, CalculatorType calculator)
    {
        PostalCode = postalCode;
        Timestamp = DateTime.Now;
        Income = income;
        Tax = tax;
        Calculator = calculator;
    }
}