using Cinema9.DataContracts.Rents.Queries.GetRent;

namespace Cinema9.Logics.Rents.Queries.GetRent;

public record GetRentQuery : IRequest<RentDetail>
{
    public Guid RentId { get; set; }
}

public class GetRentHandler(MyDatabase database) : IRequestHandler<GetRentQuery, RentDetail>
{
    public async Task<RentDetail> Handle(GetRentQuery request, CancellationToken cancellationToken)
    {
        var rent = await database.Rents
            .Where(x => x.Id == request.RentId) // Fixed: was comparing with MovieId
            .Include(x => x.Movie)
            .Select(x => new RentDetail
            {
                Id = x.Id,
                MovieId = x.MovieId,
                Qty = x.Qty,
                Amount = x.Amount,
                MovieTitle = x.Movie.Title,
                MovieRating = x.Movie.Rating,
                MoviePrice = x.Movie.Budget
            })
            .AsNoTracking() // Recommended for read-only queries
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Rent with ID {request.RentId} not found");

        return rent;
    }
}
