using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;
using Taxually.TechnicalTest.Services.Processors;

namespace Taxually.TechnicalTest.Services.Handlers;

public class FrVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyQueueClient _queueClient;
    private readonly CsvVatRegistrationProcessor _processor;

    public FrVatRegistrationHandler(ITaxuallyQueueClient queueClient, CsvVatRegistrationProcessor processor)
    {
        _queueClient = queueClient;
        _processor = processor;
    }

    public Task Handle(VatRegistrationRequest request)
    {
        var csv = _processor.Process(request);

        return _queueClient.EnqueueAsync(Constants.CsvQueue, csv);
    }
}
