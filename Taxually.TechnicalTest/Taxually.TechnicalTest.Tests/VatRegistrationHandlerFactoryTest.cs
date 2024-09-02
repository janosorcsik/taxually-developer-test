using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Extensions;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;

namespace Taxually.TechnicalTest.Tests;

public class VatRegistrationHandlerFactoryTest
{
    private readonly VatRegistrationHandlerFactory _vatRegistrationHandlerFactory;

    public VatRegistrationHandlerFactoryTest()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _vatRegistrationHandlerFactory = new VatRegistrationHandlerFactory(serviceProvider);
    }

    [Fact]
    public void When_Called_With_Invalid_Country_Then_Throws_Exception()
    {
        var request = new VatRegistrationRequest
        {
            Country = "Invalid",
        };

        var action = () => _vatRegistrationHandlerFactory.CreateHandler(request);

        action.Should().Throw<CountryNotSupportedException>();
    }

    [Theory]
    [InlineData("DE", typeof(DeVatRegistrationHandler))]
    [InlineData("FR", typeof(FrVatRegistrationHandler))]
    [InlineData("GB", typeof(GbVatRegistrationHandler))]
    public void When_Called_With_Valid_Country_Then_Returns_Handler(string country, Type handlerType)
    {
        var request = new VatRegistrationRequest
        {
            Country = country,
        };

        var handler = _vatRegistrationHandlerFactory.CreateHandler(request);

        handler.Should().BeOfType(handlerType);
    }
}
