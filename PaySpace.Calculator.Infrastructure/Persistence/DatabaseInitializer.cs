using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace PaySpace.Calculator.Infrastructure.Persistence;

internal sealed class DatabaseInitializer : IDatabaseInitializer
{
    private readonly CalculatorContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(CalculatorContext dbContext, IServiceProvider serviceProvider, ILogger<DatabaseInitializer> logger)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetPendingMigrations().Any())
        {
            _logger.LogInformation("Applying Migrations.");
            await _dbContext.Database.MigrateAsync(cancellationToken);
        }

        using var scope = _serviceProvider.CreateScope();

        await scope.ServiceProvider.GetRequiredService<CalculatorDbInitializer>()
            .InitializeAsync(cancellationToken);
    }
}