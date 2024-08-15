using Microsoft.Extensions.DependencyInjection;

namespace PaySpace.Calculator.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCalculatorServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        return services;
    }
}