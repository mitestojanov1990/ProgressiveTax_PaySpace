using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace PaySpace.Calculator.Infrastructure.Persistence;
internal sealed class CalculatorDbInitializer
{
    private readonly CalculatorContext _dbContext;
    private readonly DbSeeder _dbSeeder;
    private readonly ILogger<CalculatorDbInitializer> _logger;

    public CalculatorDbInitializer(CalculatorContext dbContext, DbSeeder dbSeeder, ILogger<CalculatorDbInitializer> logger)
    {
        _dbContext = dbContext;
        _dbSeeder = dbSeeder;
        _logger = logger;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_dbContext.Database.GetMigrations().Any())
        {
            if ((await _dbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
            {
                _logger.LogInformation("Applying Migrations for");
                await _dbContext.Database.MigrateAsync(cancellationToken);
            }

            if (await _dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogInformation("Connection to Database Succeeded.");

                await _dbSeeder.SeedDatabaseAsync(_dbContext, cancellationToken);
            }
        }
    }
}