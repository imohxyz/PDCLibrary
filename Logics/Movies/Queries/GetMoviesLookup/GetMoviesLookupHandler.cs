using Cinema9.DataContracts.Movies.Queries.GetMoviesLookup;

namespace Cinema9.Logics.Movies.Queries.GetMoviesLookup;

public record GetMoviesLookupQuery : IRequest<List<MovieLookup>>
{
}

public class GetMoviesLookupHandler(MyDatabase database) : IRequestHandler<GetMoviesLookupQuery, List<MovieLookup>>
{
    public async Task<List<MovieLookup>> Handle(GetMoviesLookupQuery request, CancellationToken cancellationToken)
    {
        var movies = await database.Movies
            .Select(x => new MovieLookup
            {
                Id = x.Id,
                Title = x.Title
            })
            .ToListAsync(cancellationToken);

        return movies;
    }
}