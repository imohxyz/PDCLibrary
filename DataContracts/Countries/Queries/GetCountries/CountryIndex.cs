namespace Cinema9.DataContracts.Countries.Queries.GetCountries;

public record CountryIndex
{
    public required Guid Id { get; init; }
    public required string Code { get; init; }
    public required string Name { get; init; }
}
