using Cinema9.DataContracts.Rents.Queries.GetRents;
using Microsoft.EntityFrameworkCore;

namespace Cinema9.Logics.Rents.Queries.GetRents;

public record GetRentsQuery : IRequest<List<RentIndex>>;

public class GetRentsHandler(MyDatabase database) : IRequestHandler<GetRentsQuery, List<RentIndex>>
{
    public async Task<List<RentIndex>> Handle(GetRentsQuery request, CancellationToken cancellationToken)
    {
        return await database.Rents
            .Include(r => r.Movie) // Eager load Movie data
            .Select(r => new RentIndex
            {
                Id = r.Id,
                MovieId = r.MovieId,
                Qty = r.Qty,
                Amount = r.Amount,
                // Add these properties to your RentIndex record if needed:
                MovieTitle = r.Movie.Title,
                MovieRating = r.Movie.Rating,
                MoviePrice = r.Movie.Budget
            })
            .AsNoTracking() // Recommended for read-only queries
            .ToListAsync(cancellationToken);
    }
}