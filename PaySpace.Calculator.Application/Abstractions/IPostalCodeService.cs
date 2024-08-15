namespace PaySpace.Calculator.Application.Abstractions;

public interface IPostalCodeService : IScopedService
{
    Task<List<PostalCodeDto>> GetPostalCodesAsync(CancellationToken cancellationToken);

    Task<PostalCodeDto> CalculatorTypeAsync(string code, CancellationToken cancellationToken);
    Task<bool> CalculatorExistsForCode(string? code);
}