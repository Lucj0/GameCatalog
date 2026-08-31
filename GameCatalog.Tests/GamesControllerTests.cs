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
    public async Task  GetGames_ReturnsAllGames()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase("GetGamesTestDb").Options;

        var context = new GameCatalogContext(options);

        var controller = new GamesController(context);

        context.Games.Add(new Game { Id = 1, Title = "Elden Ring", Price = 59.99m, Publisher = "Bandai Namco"});
        context.Games.Add(new Game { Id = 2, Title = "Hollow Knight", Price = 14.99m, Publisher = "Team Cherry"});
        context.SaveChanges();

        //Act
        var result = await controller.GetGames();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var gameList = (List<GameDto>)okResult.Value;
        Assert.Equal(2, gameList.Count);
    }
}