namespace Cinema9.DataContracts.Countries.Queries.GetCountriesLookup;

public record CountryLookup
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
