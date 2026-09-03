using GameCatalog.Entities;
using GameCatalog.Data;
using GameCatalog.DTOs;
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

    // Use DTOs as a protection layer between client and database
    [HttpGet]
    public async Task<ActionResult<List<GameDto>>> GetGames()
    {
        var games = await _context.Games.ToListAsync();

        var gameDtos = games.Select(game => new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Price = game.Price,
            Publisher = game.Publisher
        }).ToList();

        return Ok(gameDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GameDto>> GetGame(int id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        var gameDto = new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Price = game.Price,
            Publisher = game.Publisher
        };

        return Ok(gameDto);
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> CreateGame(CreateGameDto gameDto)
    {
        var game = new Game
        {
            Title = gameDto.Title,
            Price = gameDto.Price,
            Publisher = gameDto.Publisher
        };

        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        var gameToReturn = new GameDto
        {
            Id = game.Id,
            Title = game.Title,
            Price = game.Price,
            Publisher = game.Publisher
        };

        return CreatedAtAction(nameof(GetGame), new { id = game.Id }, gameToReturn);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateGame(int id, UpdateGameDto incomingGame)
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