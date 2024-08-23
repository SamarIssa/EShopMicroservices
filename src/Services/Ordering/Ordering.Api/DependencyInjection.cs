using BuildingBlocks.Exceptions.Handler;
using Carter;
namespace Ordering.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app) { 

        app.MapCarter();

        app.UseExceptionHandler(options => { });

        return app;
    }
}
