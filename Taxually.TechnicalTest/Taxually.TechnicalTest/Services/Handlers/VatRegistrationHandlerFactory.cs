using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers;

public class VatRegistrationHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly Dictionary<string, Type> HandlerTypes = new()
    {
        { "DE", typeof(DeVatRegistrationHandler) },
        { "FR", typeof(FrVatRegistrationHandler) },
        { "GB", typeof(GbVatRegistrationHandler) },
    };

    public VatRegistrationHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IVatRegistrationHandler CreateHandler(VatRegistrationRequest request)
    {
        if (HandlerTypes.TryGetValue(request.Country, out var handlerType))
        {
            return (IVatRegistrationHandler)_serviceProvider.GetRequiredService(handlerType);
        }

        throw new CountryNotSupportedException(request.Country);
    }
}
