using DI;

var builder = WebApplication.CreateBuilder(args);

// Add DI extension
builder.AddDI();

var app = builder.Build();

using (var scrope = app.Services.CreateScope())
{
    var services = scrope.ServiceProvider;
    SeedData.Initialize(services);
}

app.MapGet("/", () => "Hello Word!");

app.AddAuthenAPI();

app.Run();