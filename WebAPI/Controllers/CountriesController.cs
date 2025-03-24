using Cinema9.DataContracts.Countries.Queries.GetCountries;
using Cinema9.DataContracts.Countries.Queries.GetCountriesLookup;
using Cinema9.Logics.Countries.Queries.GetCountries;
using Cinema9.Logics.Countries.Queries.GetCountriesLookup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinema9.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CountriesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<CountryIndex>>> GetCountriesAsync()
    {
        var request = new GetCountriesQuery();

        return await sender.Send(request);
    }

    [HttpGet("Lookup")]
    public async Task<ActionResult<List<CountryLookup>>> GetCountriesLookupAsync()
    {
        var request = new GetCountriesLookupQuery();

        return await sender.Send(request);
    }
}
