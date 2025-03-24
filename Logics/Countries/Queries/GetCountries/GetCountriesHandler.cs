using Cinema9.DataContracts.Countries.Queries.GetCountries;

namespace Cinema9.Logics.Countries.Queries.GetCountries;

public record GetCountriesQuery : IRequest<List<CountryIndex>>
{
}

public class GetCountriesHandler(MyDatabase database) : IRequestHandler<GetCountriesQuery, List<CountryIndex>>
{
    public async Task<List<CountryIndex>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await database.Countries
            .Select(x => new CountryIndex
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            })
            .ToListAsync(cancellationToken);

        return countries;
    }
}
