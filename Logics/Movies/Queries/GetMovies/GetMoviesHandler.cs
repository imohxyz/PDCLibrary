using Cinema9.DataContracts.Movies.Queries.GetMovies;

namespace Cinema9.Logics.Movies.Queries.GetMovies;

public record GetMoviesQuery : IRequest<List<MovieIndex>>
{
}

public class GetMoviesHandler(MyDatabase database) : IRequestHandler<GetMoviesQuery, List<MovieIndex>>
{
    public async Task<List<MovieIndex>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await database.Movies
            .Select(x => new MovieIndex
            {
                Id = x.Id,
                Title = x.Title,
                Rating = x.Rating
            })
            .ToListAsync(cancellationToken);

        return movies;
    }
}
