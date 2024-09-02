using System.Text;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services;

public class FrVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyQueueClient _queueClient;

    public FrVatRegistrationHandler(ITaxuallyQueueClient queueClient)
    {
        _queueClient = queueClient;
    }

    public Task Handle(VatRegistrationRequest request)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
        var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());

        return _queueClient.EnqueueAsync(Constants.CsvQueue, csv);
    }
}
