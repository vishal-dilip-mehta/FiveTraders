using FiveTraders.Repository;

namespace FiveTraders.Api.Extensions;

public static class RepositoryRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ITickerRepository>(provider =>
            new TickerRepository(provider.GetRequiredService<IConfiguration>().GetConnectionString("FiveTradersDb")
                                 ?? throw new InvalidOperationException("Configure ConnectionStrings:FiveTradersDb to access investments.")));
        services.AddScoped<IStockTransactionRepository>(provider =>
            new StockTransactionRepository(provider.GetRequiredService<IConfiguration>().GetConnectionString("FiveTradersDb")
                                           ?? throw new InvalidOperationException("Configure ConnectionStrings:FiveTradersDb to access investments.")));

        return services;
    }
}
