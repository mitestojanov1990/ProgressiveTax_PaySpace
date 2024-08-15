using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PaySpace.Calculator.Application;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Infrastructure.Persistence;
using PaySpace.Calculator.Services.Exceptions;

namespace PaySpace.Calculator.Services;

public sealed class PostalCodeService : IPostalCodeService
{
    private readonly CalculatorContext _dbContext;
    private readonly IMemoryCache _memoryCache;
    public PostalCodeService(CalculatorContext dbContext, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _memoryCache = memoryCache;
    }
    public async Task<List<PostalCodeDto>> GetPostalCodesAsync(CancellationToken cancellationToken)
    {
        return await _memoryCache.GetOrCreateAsync("PostalCodes", _ => _dbContext.PostalCodes.AsNoTracking().Select(x => x.Adapt<PostalCodeDto>()).ToListAsync())!;
    }

    public async Task<PostalCodeDto> CalculatorTypeAsync(string code, CancellationToken cancellationToken)
    {
        var postalCodes = await this.GetPostalCodesAsync(cancellationToken);

        var postalCode = postalCodes.FirstOrDefault(pc => pc.Code == code);
        if (postalCode == null)
        {
            throw new CalculatorException();
        }
        return postalCode;
    }
    public async Task<bool> CalculatorExistsForCode(string? code)
    {
        return await _dbContext.PostalCodes.FirstOrDefaultAsync(x => x.Code == code) != null ? true : false;
    }
}