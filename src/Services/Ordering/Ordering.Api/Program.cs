using Ordering.Api;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
//Add services to container
builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

var app = builder.Build();

app.UseApiServices();

//Configure http request pipeline
if (app.Environment.IsDevelopment())
{
   await app.InitializeDatabaseAsync();
}

app.Run();
