using Cinema9.DataContracts.Countries.Queries.GetCountriesLookup;

namespace Cinema9.Logics.Countries.Queries.GetCountriesLookup;

public record GetCountriesLookupQuery : IRequest<List<CountryLookup>>
{
}

public class GetCountriesLookupHandler(MyDatabase database) : IRequestHandler<GetCountriesLookupQuery, List<CountryLookup>>
{
    public async Task<List<CountryLookup>> Handle(GetCountriesLookupQuery request, CancellationToken cancellationToken)
    {
        var countries = await database.Countries
            .Select(x => new CountryLookup
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);

        return countries;
    }
}