using Microsoft.Extensions.DependencyInjection;
using FixedIncome.Application.FixedIncomeSimulation;

namespace FixedIncome.CrossCutting.Extensions;

public static class MediatorExtensions
{
    public static IServiceCollection AddMediatorServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(FixedIncomeBalanceModule).Assembly));
        
        return services;
    }
}