using Xunit;
using GameCatalog.Data;
using GameCatalog.DTOs;
using GameCatalog.Entities;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GameCatalog.Tests;

public class MoviesIntegrationTests
{
    [Fact]
    public async Task GetMovies_ReturnsMovies()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.GetAsync("/movies");

        //Assert
        response.EnsureSuccessStatusCode();

        var movies = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
        Assert.NotNull(movies);
        Assert.Equal(2, movies.Count);
    }

    [Fact]
    public async Task GetMovie_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.GetAsync("/movies/3");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMovie_WithValidId_ReturnsMovies()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.GetAsync("/movies/2");

        //Assert
        response.EnsureSuccessStatusCode();

        var movie = await response.Content.ReadFromJsonAsync<MovieDto>();
        Assert.NotNull(movie);
        Assert.Equal("The Drama", movie.Title);
    }

    [Fact]
    public async Task CreateMovie_ReturnsCreated()
    {
        //Arrange
        var factory = new GameCatalogFactory();
        var client = factory.CreateClient();

        var incomingMovie = new CreateMovieDto { Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 };

        //Act
        var response = await client.PostAsJsonAsync("/movies", incomingMovie);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            var movieInDb = await context.Movies.FindAsync(1);
            Assert.NotNull(movieInDb);
            Assert.Equal("The Odyssey", movieInDb.Title);
        }
    }

    [Fact]
    public async Task UpdateMovie_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        var incomingMovie = new UpdateMovieDto { Title = "SpiderMan", Genre = "Superhero", ReleaseYear = 2026 };

        //Act
        var response = await client.PutAsJsonAsync("/movies/3", incomingMovie);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMovie_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        var incomingMovie = new UpdateMovieDto { Title = "SpiderMan", Genre = "Superhero", ReleaseYear = 2026 };

        //Act
        var response = await client.PutAsJsonAsync("/movies/2", incomingMovie);

        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            var movieInDb = await context.Movies.FindAsync(2);
            Assert.NotNull(movieInDb);
            Assert.Equal("SpiderMan", movieInDb.Title);
        }
    }

    [Fact]
    public async Task DeleteMovie_WithInvalidId_ReturnsNotFound()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.DeleteAsync("/movies/3");

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovie_WithValidId_ReturnsNoContent()
    {
        //Arrange
        var factory = new GameCatalogFactory();

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Movies.Add(new Movie { Id = 1, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 });
            context.Movies.Add(new Movie { Id = 2, Title = "The Drama", Genre = "Drama", ReleaseYear = 2026 });
            context.SaveChanges();
        }

        var client = factory.CreateClient();

        //Act
        var response = await client.DeleteAsync("/movies/1");

        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using (var scope = factory.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            var movieInDb = await context.Movies.FindAsync(1);
            Assert.Null(movieInDb);
        }
    }

    [Fact]
    public async Task CreateMovie_WithInvalidTitle_ReturnsBadRequest()
    {
        //Arrange
        var factory = new GameCatalogFactory();
        var client = factory.CreateClient();

        var invalidTitleMovie = new CreateMovieDto { Genre = "Drama", ReleaseYear = 2026 };

        //Act
        var response = await client.PostAsJsonAsync("/movies", invalidTitleMovie);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateMovie_WithInvalidGenre_ReturnsBadRequest()
    {
        //Arrange
        var factory = new GameCatalogFactory();
        var client = factory.CreateClient();

        var invalidGenreMovie = new CreateMovieDto { Title = "The Odyssey", Genre = "", ReleaseYear = 2026 };

        //Act
        var response = await client.PostAsJsonAsync("/movies", invalidGenreMovie);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateMovie_WithOverPostedId_IgnoresIdAndReturnsCreated()
    {
        //Arrange
        var factory = new GameCatalogFactory();
        var client = factory.CreateClient();

        var overPosted = new { Id = 444, Title = "The Odyssey", Genre = "Drama", ReleaseYear = 2026 };

        //Act
        var response = await client.PostAsJsonAsync("/movies", overPosted);

        //Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var movie = await response.Content.ReadFromJsonAsync<MovieDto>();
        Assert.Equal(1, movie.Id);
    }
}