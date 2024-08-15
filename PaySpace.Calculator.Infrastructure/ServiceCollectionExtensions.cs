using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaySpace.Calculator.Application.Abstractions;
using PaySpace.Calculator.Infrastructure.Mapping;
using PaySpace.Calculator.Infrastructure.Persistence;

namespace PaySpace.Calculator.Infrastructure;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        MapsterSettings.Configure();
        return services.AddPersistence(configuration)
                .AddServices();
    }

    public static async Task InitializeDatabaseAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();

        await scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>()
            .InitializeDatabaseAsync(cancellationToken);
    }

    internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.IsNullOrEmpty(configuration.GetConnectionString("CalculatorDatabase")))
        {
            throw new InvalidOperationException("DB ConnectionString is not configured.");
        }

        services.AddDbContext<CalculatorContext>(opt =>
            opt.UseSqlite(configuration.GetConnectionString("CalculatorDatabase"), x =>
            {
                x.MigrationsAssembly("PaySpace.Calculator.Migrator");
            })
        )
        .AddTransient<IDatabaseInitializer, DatabaseInitializer>()
        .AddTransient<CalculatorDbInitializer>()
        .AddTransient<DbSeeder>()
        .AddServices(typeof(ICustomSeeder), ServiceLifetime.Transient)
        .AddTransient<CustomSeederRunner>();
        return services;
    }

    internal static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddServices(typeof(ISingletonService), ServiceLifetime.Singleton)
            .AddServices(typeof(ITransientService), ServiceLifetime.Transient)
            .AddServices(typeof(IScopedService), ServiceLifetime.Scoped);

    internal static IServiceCollection AddServices(this IServiceCollection services, Type interfaceType, ServiceLifetime lifetime)
    {
        var interfaceTypes =
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => interfaceType.IsAssignableFrom(t)
                            && t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterfaces().FirstOrDefault(),
                    Implementation = t
                })
                .Where(t => t.Service is not null
                            && interfaceType.IsAssignableFrom(t.Service));

        foreach (var type in interfaceTypes)
        {
            services.AddService(type.Service!, type.Implementation, lifetime);
        }

        return services;
    }

    internal static IServiceCollection AddService(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        lifetime switch
        {
            ServiceLifetime.Transient => services.AddTransient(serviceType, implementationType),
            ServiceLifetime.Scoped => services.AddScoped(serviceType, implementationType),
            ServiceLifetime.Singleton => services.AddSingleton(serviceType, implementationType),
            _ => throw new ArgumentException("Invalid lifeTime", nameof(lifetime))
        };
}
