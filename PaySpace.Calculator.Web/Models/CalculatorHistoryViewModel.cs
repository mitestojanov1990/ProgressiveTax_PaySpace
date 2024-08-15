using PaySpace.Calculator.Web.Services.Models;

namespace PaySpace.Calculator.Web.Models;

public sealed record CalculatorHistoryViewModel
{
    public List<CalculatorHistoryDto>? CalculatorHistory { get; set; }
}