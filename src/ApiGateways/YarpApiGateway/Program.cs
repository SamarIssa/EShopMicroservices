using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add Services to container

builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        //فى العشر ثوانى ماينفعش ابعت اكتر من 5 request
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(10);
       
    });
});
var app = builder.Build();

// Configure the http request pipeline
app.UseRateLimiter();
app.MapReverseProxy();
app.Run();
