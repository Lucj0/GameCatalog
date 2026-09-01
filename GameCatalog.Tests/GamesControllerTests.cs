using Xunit;
using GameCatalog.Controllers;
using GameCatalog.Data;
using GameCatalog.Entities;
using GameCatalog.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Tests;

public class GamesControllerTests
{
    [Fact]
    public async Task GetGames_ReturnsAllGames()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.Games.Add(new Game { Id = 2, Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry" });
        context.SaveChanges();

        //Act
        var result = await controller.GetGames();

        //Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var gameList = (List<GameDto>)okObjectResult.Value;
        Assert.Equal(2, gameList.Count);
    }

    [Fact]
    public async Task GetGame_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.SaveChanges();

        //Act
        var result = await controller.GetGame(2);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetGame_WithValidId_ReturnsGame()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.SaveChanges();

        //Act
        var result = await controller.GetGame(1);

        //Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
        var gameDto = (GameDto)okObjectResult.Value;
        Assert.Equal(1, gameDto.Id);
        Assert.Equal("Elden Ring", gameDto.Title);
    }


    /* Note: the database read-backs in Create/UpdateGame are not true persistence checks.
    FindAsync checks the tracker before the database so a read-back can return the tracked object if a real save never happened.
    EF-Core In-memory provider isn't a relational database so it can't prove real persistence.
    */
    [Fact]
    public async Task CreateGame_WithValidInput_CreatesGame()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        var incomingGame = new CreateGameDto { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" };

        //Act
        var result = await controller.CreateGame(incomingGame);

        //Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdGame = (GameDto)createdAtActionResult.Value;
        Assert.Equal("Elden Ring", createdGame.Title);
        Assert.Equal(1, createdGame.Id);

        var gameInDb = await context.Games.FindAsync(1);
        Assert.NotNull(gameInDb);
        Assert.Equal("Elden Ring", gameInDb.Title);
    }

    [Fact]
    public async Task UpdateGame_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.SaveChanges();

        var updateGameDto = new UpdateGameDto { Title = "Silksong", Price = 19.99m, Publisher = "Bandai Namco" };

        //Act
        var result = await controller.UpdateGame(2, updateGameDto);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateGame_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.SaveChanges();

        var updateGameDto = new UpdateGameDto { Title = "Silksong", Price = 19.99m, Publisher = "Bandai Namco" };

        //Act
        var result = await controller.UpdateGame(1, updateGameDto);

        //Assert
        Assert.IsType<NoContentResult>(result);

        var gameInDb = await context.Games.FindAsync(1);
        Assert.NotNull(gameInDb);
        Assert.Equal("Silksong", gameInDb.Title);
    }

    [Fact]
    public async Task DeleteGame_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.SaveChanges();

        //Act
        var result = await controller.DeleteGame(2);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteGame_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
        context.SaveChanges();

        //Act
        var result = await controller.DeleteGame(1);

        //Assert
        Assert.IsType<NoContentResult>(result);

        var gameInDb = await context.Games.FindAsync(1);
        Assert.Null(gameInDb);
    }
}