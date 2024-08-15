namespace PaySpace.Calculator.Web.Services.Models;

public sealed record CalculateRequest
{
    public string? PostalCode { get; set; }

    public decimal Income { get; set; }
}