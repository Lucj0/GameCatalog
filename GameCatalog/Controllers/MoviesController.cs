using GameCatalog.Entities;
using GameCatalog.Data;
using GameCatalog.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly GameCatalogContext _context;

    public MoviesController(GameCatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieDto>>> GetMovies()
    {
        var movies = await _context.Movies.ToListAsync();

        var movieDtos = movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            ReleaseYear = movie.ReleaseYear
        }).ToList();

        return Ok(movieDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        var movieDto = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            ReleaseYear = movie.ReleaseYear
        };

        return Ok(movieDto);
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> CreateMovie(CreateMovieDto movieDto)
    {
        var movie = new Movie
        {
            Title = movieDto.Title,
            Genre = movieDto.Genre,
            ReleaseYear = movieDto.ReleaseYear
        };
        
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        var movieToReturn = new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Genre = movie.Genre,
            ReleaseYear = movie.ReleaseYear
        };

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movieToReturn);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMovie(int id, UpdateMovieDto incomingMovie)
    {
        var existingMovie = await _context.Movies.FindAsync(id);

        if (existingMovie == null)
        {
            return NotFound();
        }

        existingMovie.Title = incomingMovie.Title;
        existingMovie.Genre = incomingMovie.Genre;
        existingMovie.ReleaseYear = incomingMovie.ReleaseYear;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie ==  null)
        {
            return NotFound();
        }

        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}