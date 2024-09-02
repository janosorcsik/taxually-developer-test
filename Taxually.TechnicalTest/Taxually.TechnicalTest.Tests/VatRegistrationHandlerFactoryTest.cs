using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Extensions;
using Taxually.TechnicalTest.Services.Handlers;

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
        const string country = "Invalid";

        var action = () => _vatRegistrationHandlerFactory.CreateHandler(country);

        action.Should().Throw<CountryNotSupportedException>()
            .And.Message.Should().Contain(country);
    }

    [Theory]
    [InlineData("DE", typeof(DeVatRegistrationHandler))]
    [InlineData("FR", typeof(FrVatRegistrationHandler))]
    [InlineData("GB", typeof(GbVatRegistrationHandler))]
    public void When_Called_With_Valid_Country_Then_Returns_Handler(string country, Type handlerType)
    {
        var handler = _vatRegistrationHandlerFactory.CreateHandler(country);

        handler.Should().BeOfType(handlerType);
    }
}
