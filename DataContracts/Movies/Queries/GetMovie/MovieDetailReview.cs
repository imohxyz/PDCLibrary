namespace Cinema9.DataContracts.Movies.Queries.GetMovie;

public record MovieDetailReview
{
    public required Guid Id { get; init; }
    public required string ReviewerName { get; init; }
    public required DateTimeOffset Created { get; init; }
    public required int Score { get; init; }
    public required string Comment { get; init; }
}