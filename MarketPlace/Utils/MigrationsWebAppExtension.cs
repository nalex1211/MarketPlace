using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Utils;

public static class MigrationsWebAppExtension
{
    public static void MigrateDbContext<TDbContext>(this WebApplication app)
        where TDbContext : DbContext
    {
        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        dbContext.Database.Migrate();
    }
}
