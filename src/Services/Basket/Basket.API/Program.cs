using Discount.Grpc;
using BuildingBlocks.Messaging.MassTransit;

var builder = WebApplication.CreateBuilder(args);


//Add services to container
//Application Services
var assemply = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assemply);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assemply);

builder.Services.AddCarter();

//Data Services
builder.Services.AddMarten(opts => {
    opts.Connection(builder
        .Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.Decorate<IBasketRepository, CacheBasketRepository>();
//Decorator Pattern BasketRepository Class by implement class CacheBasketRepository which extend original class without change structure
//builder.Services.AddScoped<IBasketRepository>(provider =>
//{
//    var basketRepository=provider.GetRequiredService<IBasketRepository>();
//    return new CacheBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
//});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

//Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler= new HttpClientHandler()
    {
        ServerCertificateCustomValidationCallback=
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

//Async Communication Service for Publisher

builder.Services.AddMessageBroker(builder.Configuration);

//Cross Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

var app = builder.Build();

//Configure the http pipeline request
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();
