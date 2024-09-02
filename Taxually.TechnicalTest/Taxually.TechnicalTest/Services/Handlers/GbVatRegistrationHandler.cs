using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers;

public class GbVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyHttpClient _httpClient;

    public GbVatRegistrationHandler(ITaxuallyHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task Handle(VatRegistrationRequest request)
    {
        return _httpClient.PostAsync(Constants.UkApi, request);
    }
}
