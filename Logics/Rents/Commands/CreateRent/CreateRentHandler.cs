using Cinema9.DataContracts.Rents.Commands.CreateRent;
using Cinema9.Domain.Entities;

namespace Cinema9.Logics.Rents.Commands.CreateRent;

public record CreateRentCommand : CreateRentInput, IRequest<CreateRentOutput>
{
}

public class CreateRentHandler(MyDatabase database) : IRequestHandler<CreateRentCommand, CreateRentOutput>
{
    public async Task<CreateRentOutput> Handle(CreateRentCommand request, CancellationToken cancellationToken)
    {
        // First, get the movie to access its budget
        var movie = await database.Movies
            .Where(m => m.Id == request.MovieId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Movie with ID {request.MovieId} not found");
        
        var rent = new Rent
        {
            Qty = request.Qty,
            Amount = movie.Budget * request.Qty,
            MovieId = request.MovieId
        };

        await database.Rents.AddAsync(rent, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        var output = new CreateRentOutput
        {
            RentId = rent.Id
        };

        return output;
    }
}
