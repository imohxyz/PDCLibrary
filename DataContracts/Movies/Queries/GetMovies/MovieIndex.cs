﻿namespace Cinema9.DataContracts.Movies.Queries.GetMovies;

public record MovieIndex
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required float Rating { get; init; }
    public required string CountryName { get; init; }
}
