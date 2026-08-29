using Microsoft.EntityFrameworkCore;
using GameCatalog.Entities;

namespace GameCatalog.Data;

public class GameCatalogContext : DbContext
{
    public GameCatalogContext(DbContextOptions<GameCatalogContext> options) : base(options)
    {
    }

    public DbSet<Game> Games { get; set; }
    public DbSet<Movie> Movies { get; set; }
}