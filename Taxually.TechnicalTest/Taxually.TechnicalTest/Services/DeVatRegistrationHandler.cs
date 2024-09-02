using System.Xml.Serialization;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services;

public class DeVatRegistrationHandler : IVatRegistrationHandler
{
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;

    public DeVatRegistrationHandler(ITaxuallyQueueClient taxuallyQueueClient)
    {
        _taxuallyQueueClient = taxuallyQueueClient;
    }

    public async Task Handle(VatRegistrationRequest request)
    {
        await using var stringWriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
        serializer.Serialize(stringWriter, request);
        var xml = stringWriter.ToString();

        await _taxuallyQueueClient.EnqueueAsync(Constants.XmlQueue, xml);
    }
}
