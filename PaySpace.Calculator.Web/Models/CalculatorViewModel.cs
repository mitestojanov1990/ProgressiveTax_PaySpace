using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaySpace.Calculator.Web.Models;

public sealed record CalculatorViewModel
{
    public SelectList PostalCodes { get; set; } = default!;

    public string PostalCode { get; set; } = default!;

    public decimal Income { get; set; }
}