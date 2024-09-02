using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Taxually.TechnicalTest.Clients.Interfaces;
using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Extensions;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Handlers;

namespace Taxually.TechnicalTest.Tests;

public class VatRegistrationControllerTest
{
    private readonly Mock<ITaxuallyHttpClient> _mockedHttpClient = new();
    private readonly Mock<ITaxuallyQueueClient> _mockedQueueClient = new();

    private readonly VatRegistrationController _vatRegistrationController;

    public VatRegistrationControllerTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();

        serviceCollection.AddTransient<ITaxuallyHttpClient>(_ => _mockedHttpClient.Object);
        serviceCollection.AddTransient<ITaxuallyQueueClient>(_ => _mockedQueueClient.Object);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var vatRegistrationHandlerFactory = new VatRegistrationHandlerFactory(serviceProvider);
        _vatRegistrationController = new VatRegistrationController(vatRegistrationHandlerFactory);
    }

    [Fact]
    public async Task When_Called_With_Invalid_Country_Then_Returns_BadRequest()
    {
        var request = new VatRegistrationRequest
        {
            Country = "Invalid",
            CompanyId = Guid.NewGuid().ToString(),
            CompanyName = "Test Company",
        };

        var result = await _vatRegistrationController.Post(request);
        result.Should().BeOfType<BadRequestObjectResult>();
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

        var result = await _vatRegistrationController.Post(request);
        result.Should().BeOfType<OkResult>();

        _mockedHttpClient.Verify(x => x.PostAsync(It.Is<string>(y => y == Constants.UkApi), It.IsAny<object>()));
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

        var result = await _vatRegistrationController.Post(request);
        result.Should().BeOfType<OkResult>();

        _mockedQueueClient.Verify(x => x.EnqueueAsync(It.Is<string>(y => y == Constants.CsvQueue), It.IsAny<object>()));
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

        var result = await _vatRegistrationController.Post(request);
        result.Should().BeOfType<OkResult>();

        _mockedQueueClient.Verify(x => x.EnqueueAsync(It.Is<string>(y => y == Constants.XmlQueue), It.IsAny<object>()));
    }
}
