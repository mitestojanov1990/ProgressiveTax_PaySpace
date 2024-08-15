using Microsoft.Extensions.DependencyInjection;

using PaySpace.Calculator.Web.Services.Abstractions;

namespace PaySpace.Calculator.Web.Services;

public static class ServiceCollectionExtensions
{
    public static void AddCalculatorHttpServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICalculatorService, CalculatorService>();
        services.AddScoped<IHistoryService, HistoryService>();
        services.AddScoped<IPostalCodeService, PostalCodeService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISessionManager, SessionManager>();
    }
}