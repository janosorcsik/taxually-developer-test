using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;
using Taxually.TechnicalTest.Services.Processors;

namespace Taxually.TechnicalTest.Services.Handlers;

public class DeVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyQueueClient _queueClient;
    private readonly XmlVatRegistrationProcessor _processor;

    public DeVatRegistrationHandler(ITaxuallyQueueClient queueClient, XmlVatRegistrationProcessor processor)
    {
        _queueClient = queueClient;
        _processor = processor;
    }

    public async Task Handle(VatRegistrationRequest request)
    {
        var xml = await _processor.Process(request);

        await _queueClient.EnqueueAsync(Constants.XmlQueue, xml);
    }
}
