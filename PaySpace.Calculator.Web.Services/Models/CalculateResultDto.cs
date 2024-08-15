namespace PaySpace.Calculator.Web.Services.Models;

public sealed record CalculateResultDto
{
    public string Calculator { get; set; }

    public decimal Tax { get; set; }
}