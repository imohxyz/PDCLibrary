namespace Cinema9.Domain.Entities;

public class Rent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required int Qty { get; set; }
    public required decimal Amount { get; set; }

    public required Guid MovieId { get; set; }
    public Movie Movie { get; set; } = default!;

    // public ICollection<Review> Reviews { get; set; } = [];
}
