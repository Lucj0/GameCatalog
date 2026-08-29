using GameCatalog.Entities;
using GameCatalog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Controllers;

[ApiController]
[Route("[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameCatalogContext _context;

    public GamesController(GameCatalogContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Game>>> GetGames()
    {
        var games = await _context.Games.ToListAsync();

        return Ok(games);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Game>> GetGame(int id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [HttpPost]
    public async Task<ActionResult<Game>> CreateGame(Game game)
    {
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGame(int id, Game incomingGame)
    {
        var existingGame = await _context.Games.FindAsync(id);

        if (existingGame == null)
        {
            return NotFound();
        }

        existingGame.Title = incomingGame.Title;
        existingGame.Price = incomingGame.Price;
        existingGame.Publisher = incomingGame.Publisher;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        var existingGame = await _context.Games.FindAsync(id);

        if (existingGame == null)
        {
            return NotFound();
        }

        _context.Games.Remove(existingGame);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}