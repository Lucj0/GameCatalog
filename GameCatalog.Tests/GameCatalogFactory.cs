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
            // 1. Remove the app's real DbContext registration
            services.RemoveAll<DbContextOptions<GameCatalogContext>>();

            // 2. Create and open a connection, kept alive
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            // 3. Register the DbContext to use that open connection
            services.AddDbContext<GameCatalogContext>(options =>
                options.UseSqlite(connection));

            // 4. Build the schema in the in-memory database
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<GameCatalogContext>();
            context.Database.EnsureCreated();
        });
    }
}