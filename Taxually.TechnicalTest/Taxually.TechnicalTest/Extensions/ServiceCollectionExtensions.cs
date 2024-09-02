using Taxually.TechnicalTest.Clients;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Services;

namespace Taxually.TechnicalTest.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
        services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();

        services.AddSingleton<VatRegistrationHandlerFactory>();

        services.AddVatRegistrationHandlers();

        return services;
    }

    private static void AddVatRegistrationHandlers(this IServiceCollection services)
    {
        services.AddTransient<DeVatRegistrationHandler>();
        services.AddTransient<FrVatRegistrationHandler>();
        services.AddTransient<GbVatRegistrationHandler>();
    }
}
