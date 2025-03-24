﻿using Cinema9.DataContracts.Movies.Commands.CreateMovie;
using Cinema9.DataContracts.Movies.Commands.UpdateMovie;
using Cinema9.DataContracts.Movies.Queries.GetMovie;
using Cinema9.DataContracts.Movies.Queries.GetMovies;
using Cinema9.DataContracts.Movies.Queries.GetMoviesLookup;
using Cinema9.Logics.Common.Exceptions;
using Cinema9.Logics.Movies.Commands.CreateMovie;
using Cinema9.Logics.Movies.Commands.DeleteMovie;
using Cinema9.Logics.Movies.Commands.UpdateMovie;
using Cinema9.Logics.Movies.Queries.GetMovie;
using Cinema9.Logics.Movies.Queries.GetMovies;
using Cinema9.Logics.Movies.Queries.GetMoviesLookup;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinema9.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<MovieIndex>>> GetMoviesAsync()
    {
        var request = new GetMoviesQuery();

        return await sender.Send(request);
    }

    [HttpGet("Lookup")]
    public async Task<ActionResult<List<MovieLookup>>> GetMoviesLookupAsync()
    {
        var request = new GetMoviesLookupQuery();

        return await sender.Send(request);
    }

    [HttpGet("{movieId:guid}")]
    public async Task<ActionResult<MovieDetail>> GetMovieAsync(Guid movieId)
    {
        try
        {
            var request = new GetMovieQuery
            {
                MovieId = movieId
            };

            var movie = await sender.Send(request);

            return movie;
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<CreateMovieOutput>> CreateMovieAsync([FromBody] CreateMovieInput input)
    {
        var request = new CreateMovieCommand
        {
            Title = input.Title,
            ReleaseDate = input.ReleaseDate,
            Budget = input.Budget
        };

        var output = await sender.Send(request);

        return output;
    }

    [HttpPut("{movieId:guid}")]
    public async Task<ActionResult> UpdateMovieAsync(Guid movieId, [FromBody] UpdateMovieInput input)
    {
        Console.WriteLine($"input.Id: {input.Id}");
        Console.WriteLine($"input.Title: {input.Title}");
        Console.WriteLine($"input.Synopsis: {input.Synopsis}");
        Console.WriteLine($"input.ReleaseDate: {input.ReleaseDate}");
        Console.WriteLine($"input.Rating: {input.Rating}");
        Console.WriteLine($"input.Budget: {input.Budget}");

        if (movieId != input.Id)
        {
            return BadRequest();
        }

        try
        {
            var request = new UpdateMovieCommand
            {
                Id = input.Id,
                Title = input.Title,
                Synopsis = input.Synopsis,
                ReleaseDate = input.ReleaseDate,
                Rating = input.Rating,
                Budget = input.Budget
            };

            await sender.Send(request);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{movieId:guid}")]
    public async Task<ActionResult> DeleteMovie(Guid movieId)
    {
        try
        {
            var request = new DeleteMovieCommand
            {
                MovieId = movieId
            };

            await sender.Send(request);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
