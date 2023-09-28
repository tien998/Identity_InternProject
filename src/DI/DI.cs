using IdentityServices.Authentication;
using Microsoft.EntityFrameworkCore;

namespace DI;

public static class WebAppBuilderExtension
{
    /// <summary>
    /// Extension method that Inject Service to DI
    /// </summary>
    public static void AddDI(this WebApplicationBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("dbContext")!;
        builder.Services.AddDbContext<AuthenDb>(opts =>
        {
            opts.UseSqlServer(connectionString);
        });
        // builder.Services.AddSingleton<DbContext, AuthenDb>();
        builder.Services.AddScoped<AuthenManager>();
    }
}