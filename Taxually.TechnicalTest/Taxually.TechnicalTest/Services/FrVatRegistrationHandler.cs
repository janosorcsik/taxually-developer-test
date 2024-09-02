using System.Text;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services;

public class FrVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;

    public FrVatRegistrationHandler(ITaxuallyQueueClient taxuallyQueueClient)
    {
        _taxuallyQueueClient = taxuallyQueueClient;
    }

    public Task Handle(VatRegistrationRequest request)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
        var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
        // Queue file to be processed
        return _taxuallyQueueClient.EnqueueAsync(Constants.CsvQueue, csv);
    }
}
