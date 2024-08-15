using PaySpace.Calculator.Application.DTO;

namespace PaySpace.Calculator.Application.Abstractions;

public interface IHistoryService : IScopedService
{
    Task<IEnumerable<CalculatorHistoryDto>> GetHistoryAsync(CancellationToken cancellationToken);

    Task AddAsync(CalculatorHistoryDto calculatorHistory, CancellationToken cancellationToken);
}