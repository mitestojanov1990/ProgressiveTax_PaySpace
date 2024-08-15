namespace PaySpace.Calculator.Web.Services.Models;

public sealed record PostalCodeDto
{
    public string Code { get; set; } = default!;
    public string Calculator { get; set; } = default!;
}