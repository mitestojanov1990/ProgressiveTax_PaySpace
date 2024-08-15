namespace PaySpace.Calculator.Application.Abstractions;
public interface ICalculationService : IScopedService
{
    Task<CalculateResultDto> CalculateAsync(CalculateRequest request, CancellationToken cancellationToken);
}
