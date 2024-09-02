using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Exceptions;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Handlers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VatRegistrationController : ControllerBase
{
    private readonly VatRegistrationHandlerFactory _vatRegistrationHandlerFactory;

    public VatRegistrationController(VatRegistrationHandlerFactory vatRegistrationHandlerFactory)
    {
        _vatRegistrationHandlerFactory = vatRegistrationHandlerFactory;
    }

    /// <summary>
    /// Registers a company for a VAT number in a given country
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
    {
        try
        {
            var handler = _vatRegistrationHandlerFactory.CreateHandler(request.Country);

            await handler.Handle(request);
        }
        catch (CountryNotSupportedException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
}
