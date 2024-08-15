using Mapster;
using Microsoft.EntityFrameworkCore;
using PaySpace.Calculator.Application;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Domain;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Infrastructure.Persistence;

namespace PaySpace.Calculator.Services;

public sealed class HistoryService : IHistoryService
{
    private readonly CalculatorContext _dbContext;
    public HistoryService(CalculatorContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task AddAsync(CalculatorHistoryDto history, CancellationToken cancellationToken)
    {
        var calculatorType = (CalculatorType)Enum.Parse(typeof(CalculatorType), history.Calculator);
        var calculatorHistory = new CalculatorHistory(history.PostalCode, history.Income, history.Tax, calculatorType);
        await _dbContext.AddAsync(calculatorHistory, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<CalculatorHistoryDto>> GetHistoryAsync(CancellationToken cancellationToken) => await _dbContext.CalculatorHistories.OrderByDescending(x => x.Timestamp).Select(x => x.Adapt<CalculatorHistoryDto>()).ToListAsync(cancellationToken);
}