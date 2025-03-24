using Cinema9.DataContracts.Movies.Queries.GetMovie;

namespace Cinema9.Logics.Movies.Queries.GetMovie;

public record GetMovieQuery : IRequest<MovieDetail>
{
    public Guid MovieId { get; set; }
}

public class GetMovieHandler(MyDatabase database) : IRequestHandler<GetMovieQuery, MovieDetail>
{
    public async Task<MovieDetail> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await database.Movies
            .Where(x => x.Id == request.MovieId)
            .Select(x => new MovieDetail
            {
                Id = x.Id,
                Title = x.Title,
                Synopsis = x.Synopsis,
                ReleaseDate = x.ReleaseDate,
                Rating = x.Rating,
                Budget = x.Budget,
                CountryId = x.CountryId,
                CountryCode = x.Country.Code,
                CountryName = x.Country.Name
            })
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Movie not found");

        return movie;
    }
}
