using Xunit;
using GameCatalog.Controllers;
using GameCatalog.Data;
using GameCatalog.DTOs;
using GameCatalog.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Tests;

public class MoviesControllerTests
{
    //GetMovies
    [Fact]
    public async Task GetMovies_ReturnsAllMovies()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        //Act
        var result = await controller.GetMovies();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var moviesList = (List<MovieDto>)okResult.Value;
        Assert.Equal(2, moviesList.Count);
    }


    //GetMovie
    [Fact]
    public async Task GetMovie_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        //Act
        var result = await controller.GetMovie(3);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetMovie_WithValidId_ReturnsMovie()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        //Act
        var result = await controller.GetMovie(2);

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var movie = (MovieDto)okResult.Value;
        Assert.Equal("The Drama", movie.Title);
    }


    //CreateMovie
    [Fact]
    public async Task CreateMovie_ReturnsCreatedAtAction()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        var incomingMovie = new CreateMovieDto { Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 };

        //Act
        var result = await controller.CreateMovie(incomingMovie);

        //Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdMovie = (MovieDto)createdAtActionResult.Value;
        Assert.Equal("The Odyssey", createdMovie.Title);
        Assert.Equal(1, createdMovie.Id);

        var movieInDb = await context.Movies.FindAsync(1);
        Assert.NotNull(movieInDb);
        Assert.Equal("The Odyssey", movieInDb.Title);
    }

    //UpdateMovie
    [Fact]
    public async Task UpdateMovie_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        var updateMovieDto = new UpdateMovieDto { Title = "Project Hail Mary", Genre = "Sci-fi", ReleaseYear = 2026 };

        //Act
        var result = await controller.UpdateMovie(3, updateMovieDto);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task UpdateMovie_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        var updateMovieDto = new UpdateMovieDto { Title = "Project Hail Mary", Genre = "Sci-fi", ReleaseYear = 2026 };

        //Act
        var result = await controller.UpdateMovie(2, updateMovieDto);

        //Assert
        Assert.IsType<NoContentResult>(result);

        var movieInDb = await context.Movies.FindAsync(2);
        Assert.NotNull(movieInDb);
        Assert.Equal("Project Hail Mary", movieInDb.Title);
    }


    //DeleteMovie
    [Fact]
    public async Task DeleteMovie_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        //Act
        var result = await controller.DeleteMovie(3);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteMovie_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var options = new DbContextOptionsBuilder<GameCatalogContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

        var context = new GameCatalogContext(options);

        var controller = new MoviesController(context);

        context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
        context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
        context.SaveChanges();

        //Act
        var result = await controller.DeleteMovie(2);

        //Assert
        Assert.IsType<NoContentResult>(result);
        
        var movieInDb = await context.Movies.FindAsync(2);
        Assert.Null(movieInDb);
    }

}