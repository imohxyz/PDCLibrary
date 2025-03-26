namespace Cinema9.DataContracts.Rents.Commands.CreateRent;

public record CreateRentOutput
{
    public required Guid RentId { get; init; }
}
