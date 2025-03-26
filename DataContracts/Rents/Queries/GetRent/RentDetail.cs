namespace Cinema9.DataContracts.Rents.Queries.GetRent;

public record RentDetail
{
    public required Guid Id { get; init; }
    public required Guid MovieId { get; init; }
    public required int Qty { get; init; }
    public required decimal Amount { get; init; }
    // public required DateTimeOffset CreatedDate { get; init; }

    public string? MovieTitle { get; init; }
    public float? MovieRating { get; init; }
    public decimal? MoviePrice { get; init; }
}
