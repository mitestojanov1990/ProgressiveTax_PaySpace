using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PaySpace.Calculator.Domain;
using PaySpace.Calculator.Domain.Enum;

namespace PaySpace.Calculator.Infrastructure.Persistence;
internal sealed class DbSeeder
{
    private readonly CustomSeederRunner _seederRunner;
    private readonly ILogger<DbSeeder> _logger;

    public DbSeeder(ILogger<DbSeeder> logger, CustomSeederRunner seederRunner)
    {
        _logger = logger;
        _seederRunner = seederRunner;
    }

    public async Task SeedDatabaseAsync(CalculatorContext dbContext, CancellationToken cancellationToken)
    {
        await SeedPostalCodes(dbContext);
        await SeedCalculatorSettings(dbContext);

        await _seederRunner.RunSeedersAsync(cancellationToken);
    }

    private async Task SeedPostalCodes(CalculatorContext dbContext)
    {
        if (await dbContext.PostalCodes.AnyAsync())
        {
            _logger.LogInformation("PostalCodes already exist, skipping seeding.");
            return;
        }
        var postalCodes = new List<PostalCode>()
        {
            new() { Calculator = CalculatorType.Progressive, Code = "7441" },
            new() { Calculator = CalculatorType.FlatValue, Code = "A100" },
            new() { Calculator = CalculatorType.FlatRate, Code = "7000" },
            new() { Calculator = CalculatorType.Progressive, Code = "1000" },
        };
        foreach (var postalCode in postalCodes)
        {
            _logger.LogInformation("Seeding {code} postal code for calculator '{calculator}'.", postalCode.Code, postalCode.Calculator.ToString());
            dbContext.PostalCodes.Add(postalCode);
            await dbContext.SaveChangesAsync();
        }
    }

    private async Task SeedCalculatorSettings(CalculatorContext dbContext)
    {
        if (await dbContext.CalculatorSettings.AnyAsync())
        {
            _logger.LogInformation("CalculatorSettings already exist, skipping seeding.");
            return;
        }
        var calculatorSettings = new List<CalculatorSetting>()
        {
            new() { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 10, From = 0, To = 8350 },
            new() { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 15, From = 8351, To = 33950 },
            new() { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 25, From = 33951, To = 82250 },
            new() { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 28, From = 82251, To = 171550 },
            new() { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 33, From = 171551, To = 372950 },
            new() { Calculator = CalculatorType.Progressive, RateType = RateType.Percentage, Rate = 35, From = 372951, To = null },
            new() { Calculator = CalculatorType.FlatValue, RateType = RateType.Percentage, Rate = 5, From = 0, To = 199999 },
            new() { Calculator = CalculatorType.FlatValue, RateType = RateType.Amount, Rate = 10000, From = 200000, To = null },
            new() { Calculator = CalculatorType.FlatRate, RateType = RateType.Percentage, Rate = 17.5M, From = 0, To = null },
        };
        foreach (var setting in calculatorSettings)
        {
            _logger.LogInformation("Seeding {rateType} rate type with {rate} and rate type {rateType} for calculator '{calculator}'.", 
                setting.RateType, setting.Rate, setting.RateType, setting.Calculator.ToString());
            dbContext.CalculatorSettings.Add(setting);
            await dbContext.SaveChangesAsync();
        }
    }
}
