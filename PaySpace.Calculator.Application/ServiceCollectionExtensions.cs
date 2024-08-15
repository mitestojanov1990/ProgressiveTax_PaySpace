using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PaySpace.Calculator.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);

        var assembly = Assembly.GetExecutingAssembly();
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
