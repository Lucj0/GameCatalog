using GameCatalog.Entities;
using GameCatalog.Data;
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
    public async Task<ActionResult<List<Movie>>> GetMovies()
    {
        var movies = await _context.Movies.ToListAsync();

        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _context.Movies.FindAsync(id);

        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMovie), new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateMovie(int id, Movie incomingMovie)
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