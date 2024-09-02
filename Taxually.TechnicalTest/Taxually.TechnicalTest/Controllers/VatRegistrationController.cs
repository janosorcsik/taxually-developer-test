﻿using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VatRegistrationController : ControllerBase
{
    private readonly ITaxuallyHttpClient _taxuallyHttpClient;
    private readonly ITaxuallyQueueClient _taxuallyQueueClient;

    public VatRegistrationController(ITaxuallyHttpClient taxuallyHttpClient, ITaxuallyQueueClient taxuallyQueueClient)
    {
        _taxuallyHttpClient = taxuallyHttpClient;
        _taxuallyQueueClient = taxuallyQueueClient;
    }

    /// <summary>
    /// Registers a company for a VAT number in a given country
    /// </summary>
    /// <exception cref="CountryNotSupportedException"></exception>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
    {
        switch (request.Country)
        {
            case "GB":
                // UK has an API to register for a VAT number
                await _taxuallyHttpClient.PostAsync("https://api.uktax.gov.uk", request);
                break;
            case "FR":
                // France requires an Excel spreadsheet to be uploaded to register for a VAT number
                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("CompanyName,CompanyId");
                csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
                var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                // Queue file to be processed
                await _taxuallyQueueClient.EnqueueAsync("vat-registration-csv", csv);
                break;
            case "DE":
                // Germany requires an XML document to be uploaded to register for a VAT number
                await using (var stringWriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                    serializer.Serialize(stringWriter, request);
                    var xml = stringWriter.ToString();
                    // Queue xml doc to be processed
                    await _taxuallyQueueClient.EnqueueAsync("vat-registration-xml", xml);
                }

                break;
            default:
                throw new CountryNotSupportedException();
        }

        return Ok();
    }
}
