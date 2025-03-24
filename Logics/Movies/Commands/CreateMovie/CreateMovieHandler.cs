using Cinema9.DataContracts.Movies.Commands.CreateMovie;
using Cinema9.Domain.Entities;

namespace Cinema9.Logics.Movies.Commands.CreateMovie;

public record CreateMovieCommand : CreateMovieInput, IRequest<CreateMovieOutput>
{
}

public class CreateMovieHandler(MyDatabase database) : IRequestHandler<CreateMovieCommand, CreateMovieOutput>
{
    public async Task<CreateMovieOutput> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie
        {
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Budget = request.Budget,
            Synopsis = null,
            Rating = 0.0F,
            CountryId = request.CountryId
        };

        await database.Movies.AddAsync(movie, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        var output = new CreateMovieOutput
        {
            MovieId = movie.Id
        };

        return output;
    }
}
