using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Application.DTO;
using PaySpace.Calculator.Domain.Enum;
using PaySpace.Calculator.Infrastructure.Persistence;

namespace PaySpace.Calculator.Services;

public sealed class CalculatorSettingsService : ICalculatorSettingsService
{
    private readonly CalculatorContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    public CalculatorSettingsService(CalculatorContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }
    public Task<List<CalculatorSettingDto>> GetSettingsAsync(string calculatorTypeStr, CancellationToken cancellationToken)
    {
        var calculatorType = (CalculatorType)Enum.Parse(typeof(CalculatorType), calculatorTypeStr);
        return _memoryCache.GetOrCreateAsync($"CalculatorSetting:{calculatorTypeStr}", entry =>
        {
            return _dbContext.CalculatorSettings.AsNoTracking().Where(_ => _.Calculator == calculatorType).Select(x => x.Adapt<CalculatorSettingDto>()).ToListAsync(cancellationToken);
        })!;
    }
}