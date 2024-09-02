using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services;

public class GbVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyHttpClient _taxuallyHttpClient;

    public GbVatRegistrationHandler(ITaxuallyHttpClient taxuallyHttpClient)
    {
        _taxuallyHttpClient = taxuallyHttpClient;
    }

    public Task Handle(VatRegistrationRequest request)
    {
        return _taxuallyHttpClient.PostAsync(Constants.UkApi, request);
    }
}
