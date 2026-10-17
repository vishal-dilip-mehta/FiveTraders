using FiveTraders.Service;

namespace FiveTraders.Api.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IInvestmentsService, InvestmentsService>();

        return services;
    }
}
