using System.Data.Common;
using GameCatalog.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GameCatalog.Tests;

public class GameCatalogFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's real DbContext registration
            services.RemoveAll<DbContextOptions<GameCatalogContext>>();

            // 2. Create and open a connection, kept alive for lifetime of the factory
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // Register the DbContext to use the open connection
            services.AddDbContext<GameCatalogContext>(options =>
                options.UseSqlite(connection));

            // Fresh in-memory database has no tables.
            // Build the schema in the in-memory database so the app's queries have something to hit
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Database.EnsureCreated();
        });
    }
}