using Microsoft.Extensions.DependencyInjection;

namespace FixedIncome.Application.Configuration;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        
        return services;
    }
}