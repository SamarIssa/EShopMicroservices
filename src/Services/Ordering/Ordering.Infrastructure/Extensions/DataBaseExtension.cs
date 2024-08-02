using Microsoft.AspNetCore.Builder;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Ordering.Infrastructure.Extensions;

public static class DataBaseExtension
{
    public static async Task InitializeDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.MigrateAsync().GetAwaiter().GetResult();
    }
}
