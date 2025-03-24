using Cinema9.DataContracts.Movies.Commands.UpdateMovie;

namespace Cinema9.Logics.Movies.Commands.UpdateMovie;

public record UpdateMovieCommand : UpdateMovieInput, IRequest
{
}

public class UpdateMovieHandler(MyDatabase database) : IRequestHandler<UpdateMovieCommand>
{
    public async Task Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await database.Movies
            .Where(x => x.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Movie not found.");

        movie.Title = request.Title;
        movie.Synopsis = request.Synopsis;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Budget = request.Budget;
        movie.CountryId = request.CountryId;

        await database.SaveChangesAsync(cancellationToken);
    }
}
