using System.Text;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services.Processors;

public class CsvVatRegistrationProcessor
{
    public byte[] Process(VatRegistrationRequest request)
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("CompanyName,CompanyId");
        csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");

        return Encoding.UTF8.GetBytes(csvBuilder.ToString());
    }
}
