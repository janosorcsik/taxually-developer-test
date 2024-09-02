using Taxually.TechnicalTest.Clients;
using Taxually.TechnicalTest.Clients.Interfaces;

namespace Taxually.TechnicalTest.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
        services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();

        return services;
    }
}
