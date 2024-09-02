using System.Xml.Serialization;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services.Processors;

public class XmlVatRegistrationProcessor
{
    public async Task<string> Process(VatRegistrationRequest request)
    {
        await using var stringWriter = new StringWriter();
        var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
        serializer.Serialize(stringWriter, request);

        return stringWriter.ToString();
    }
}
