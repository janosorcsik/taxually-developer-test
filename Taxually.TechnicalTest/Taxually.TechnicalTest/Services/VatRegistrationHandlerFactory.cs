using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services;

public class VatRegistrationHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public VatRegistrationHandlerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IVatRegistrationHandler CreateHandler(VatRegistrationRequest request)
    {
        return request.Country switch
        {
            "GB" => _serviceProvider.GetRequiredService<GbVatRegistrationHandler>(),
            "FR" => _serviceProvider.GetRequiredService<FrVatRegistrationHandler>(),
            "DE" => _serviceProvider.GetRequiredService<DeVatRegistrationHandler>(),
            _ => throw new CountryNotSupportedException()
        };
    }
}
