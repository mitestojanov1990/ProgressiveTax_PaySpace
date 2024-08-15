using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PaySpace.Calculator.Application;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Infrastructure;
using PaySpace.Calculator.Services;
using Serilog;

namespace PaySpace.Calculator.Benchmarks;

[MemoryDiagnoser]
public class CalculationBenchmark
{
    private readonly ICalculationService _calculationService;
    private readonly CalculateRequest _progressiveRequest;

    public CalculationBenchmark()
    {

        var serviceCollection = new ServiceCollection();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();


        serviceCollection.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.AddSerilog();
        });

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        serviceCollection.AddInfrastructure(configuration);
        serviceCollection.AddApplication();
        serviceCollection.AddCalculatorServices();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        serviceProvider.InitializeDatabaseAsync().GetAwaiter().GetResult();

        _calculationService = serviceProvider.GetRequiredService<ICalculationService>();
    }

    [Benchmark]
    public async Task<CalculateResultDto> BenchmarkProgressiveCalculation()
    {
        var request = new CalculateRequest("7441", 50000m);
        var result = await _calculationService.CalculateAsync(request, CancellationToken.None);
        return result;
    }
}