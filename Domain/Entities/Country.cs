namespace Cinema9.Domain.Entities;

public class Country
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Code { get; set; }
    public required string Name { get; set; }

    public ICollection<Movie> Movies { get; set; } = [];
}
