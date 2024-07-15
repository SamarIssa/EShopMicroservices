

var builder = WebApplication.CreateBuilder(args);


//Add services to container
var assemply = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assemply);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assemply);

builder.Services.AddCarter();

builder.Services.AddMarten(opts => {
    opts.Connection(builder
        .Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();
if(builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();


builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

//Configure the http pipeline request
app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
