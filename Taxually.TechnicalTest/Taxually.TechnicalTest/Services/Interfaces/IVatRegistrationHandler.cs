using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services.Interfaces;

public interface IVatRegistrationHandler
{
    Task Handle(VatRegistrationRequest request);
}
