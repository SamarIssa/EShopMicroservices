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
        await SeedAsync(dbContext);
    }

    private static async Task SeedAsync(ApplicationDbContext dbContext)
    {
        await SeedCustomerAsync(dbContext);
        await SeedProductAsync(dbContext);
        await SeedOrderAndItemsAsync(dbContext);

    }

    private static async Task SeedOrderAndItemsAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Orders.AnyAsync())
        {
            await dbContext.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedProductAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Products.AnyAsync())
        {
            await dbContext.Products.AddRangeAsync(InitialData.Products);
            await dbContext.SaveChangesAsync();
        }
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext dbContext)
    {
        if (!await dbContext.Customers.AnyAsync()) { 
           
            await dbContext.Customers.AddRangeAsync(InitialData.Customers);
            await dbContext.SaveChangesAsync();
        }
    }
}
