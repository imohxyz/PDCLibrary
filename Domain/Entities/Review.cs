namespace Cinema9.Domain.Entities;

public class Review
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string ReviewerName { get; set; }
    public required int Score { get; set; }
    public required string Comment { get; set; }
    public required DateTimeOffset Created { get; set; }

    public required Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;
}
