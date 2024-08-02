

using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure;

//We will add dependency injection in every layer for separation of concern
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<ApplicationDbContext>(options=>options.UseSqlServer(connectionString));
        return services;
    }
    //Command for Add migration in specific dir
   // Add-Migration InitialCreate -outputDir Data/Migrations -project Ordering.Infrastructure -StartupProject Ordering.Api
}
