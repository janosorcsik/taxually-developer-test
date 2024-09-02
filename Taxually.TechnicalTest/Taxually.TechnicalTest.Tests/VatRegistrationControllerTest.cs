using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Tests;

public class VatRegistrationControllerTest
{
    private readonly Mock<ITaxuallyHttpClient> _mockedTaxuallyHttpClient = new();
    private readonly Mock<ITaxuallyQueueClient> _mockedTaxuallyQueueClient = new();

    private readonly VatRegistrationController _vatRegistrationController;

    public VatRegistrationControllerTest()
    {
        _vatRegistrationController = new VatRegistrationController(_mockedTaxuallyHttpClient.Object, _mockedTaxuallyQueueClient.Object);
    }

    [Fact]
    public async Task When_Called_With_InValid_Country_Then_Throws_Exception()
    {
        var request = new VatRegistrationRequest
        {
            Country = "HU",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "Test Company",
        };

        Func<Task> action = () => _vatRegistrationController.Post(request);

        await action.Should().ThrowAsync<CountryNotSupportedException>();
    }

    [Fact]
    public async Task When_Called_With_UK_Then_Returns_Success_And_Calls_HttpClient()
    {
        var request = new VatRegistrationRequest
        {
            Country = "GB",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "Test Company",
        };

        var result =  await _vatRegistrationController.Post(request);
        result.Should().BeOfType<OkResult>();

        _mockedTaxuallyHttpClient.Verify(x => x.PostAsync(It.Is<string>(y => y == Constants.UkApi), It.IsAny<object>()));
    }

    [Fact]
    public async Task When_Called_With_French_Then_Returns_Success_And_Calls_QueueClient()
    {
        var request = new VatRegistrationRequest
        {
            Country = "FR",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "Test Company",
        };

        var result =  await _vatRegistrationController.Post(request);
        result.Should().BeOfType<OkResult>();

        _mockedTaxuallyQueueClient.Verify(x => x.EnqueueAsync(It.Is<string>(y => y == Constants.CsvQueue), It.IsAny<object>()));
    }

    [Fact]
    public async Task When_Called_With_Germany_Then_Returns_Success_And_Calls_QueueClient()
    {
        var request = new VatRegistrationRequest
        {
            Country = "DE",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "Test Company",
        };

        var result =  await _vatRegistrationController.Post(request);
        result.Should().BeOfType<OkResult>();

        _mockedTaxuallyQueueClient.Verify(x => x.EnqueueAsync(It.Is<string>(y => y == Constants.XmlQueue), It.IsAny<object>()));
    }
}
