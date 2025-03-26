namespace Cinema9.DataContracts.Rents.Commands.CreateRent;

public record CreateRentInput
{
    public required int Qty { get; init; }
    // public required float Amount { get; init; }
    public required Guid MovieId { get; set; }
}
