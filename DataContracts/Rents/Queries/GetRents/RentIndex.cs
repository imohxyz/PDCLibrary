namespace Cinema9.DataContracts.Rents.Queries.GetRents;

public record RentIndex
{
    public required Guid Id { get; init; }
    public required Guid MovieId { get; init; }
    public required int Qty { get; init; }
    public required decimal Amount { get; init; }
    
    // Optional movie details
    public string? MovieTitle { get; init; }
    public float? MovieRating { get; init; }
    public decimal? MoviePrice { get; init; }
}