using Xunit;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using GameCatalog.Data;
using GameCatalog.Entities;
using GameCatalog.DTOs;

namespace GameCatalog.Tests;

public class GamesIntegrationTests
{
    [Fact]
    public async Task GetGames_ReturnsSuccessAndGames()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.Games.Add(new Game { Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.GetAsync("/games");

        //Assert
        response.EnsureSuccessStatusCode();
        var games = await response.Content.ReadFromJsonAsync<List<GameDto>>();
        Assert.NotNull(games);
        Assert.Equal(2, games.Count);
    }

    [Fact]
    public async Task GetGame_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.GetAsync("/games/2");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetGame_WithValidId_ReturnsGame()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.GetAsync("/games/1");

        //Assert
        response.EnsureSuccessStatusCode();
        var game = await response.Content.ReadFromJsonAsync<GameDto>();
        Assert.NotNull(game);
        Assert.Equal("Elden Ring", game.Title);
    }

    [Fact]
    public async Task CreateGame_ReturnsCreatedAtAction()
    {
        //Arrange
        var factory = new GameCatalogFactory();
        var client = factory.CreateClient();

        var incomingGame = new CreateGameDto { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" };

        //Act
        var response = await client.PostAsJsonAsync("/games", incomingGame);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var returnedGame = await response.Content.ReadFromJsonAsync<GameDto>();
        Assert.Equal("Elden Ring", returnedGame.Title);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            var gameInDb = await context.Games.FindAsync(1);
            Assert.NotNull(gameInDb);
            Assert.Equal("Elden Ring", gameInDb.Title);
        }
    }

    [Fact]
    public async Task UpdateGame_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.Games.Add(new Game { Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        var incomingGame = new UpdateGameDto { Title = "GTA 6", Price = 79.99m, Publisher = "Rockstar" };

        //Act
        var response = await client.PutAsJsonAsync("/games/3", incomingGame);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateGame_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.Games.Add(new Game { Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        var incomingGame = new UpdateGameDto { Title = "GTA 6", Price = 79.99m, Publisher = "Rockstar" };

        //Act
        var response = await client.PutAsJsonAsync("/games/2", incomingGame);

        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            var gameInDb = await context.Games.FindAsync(2);
            Assert.NotNull(gameInDb);
            Assert.Equal("GTA 6", gameInDb.Title);
        }
    }

    [Fact]
    public async Task DeleteGame_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.Games.Add(new Game { Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.DeleteAsync("/games/3");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteGame_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Games.Add(new Game { Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco" });
            context.Games.Add(new Game { Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry" });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.DeleteAsync("/games/2");

        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            var gameInDb = await context.Games.FindAsync(2);
            Assert.Null(gameInDb);
        }
    }
}