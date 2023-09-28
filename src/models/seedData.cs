using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new AuthenDb(
            serviceProvider.GetRequiredService<
                DbContextOptions<AuthenDb>>()))
        {
            // Look for any movies.MvcMovieContext
            if (context.Role.Any())
            {
                return;   // DB has been seeded
            }
            context.Role.AddRange(
                new[]{
                    new Model.AuthenModels.Role()
                    {
                        Id="sa",
                        RoleName="Supper Admin",
                        Description="Ultimate Right Admin",
                    },
                    new Model.AuthenModels.Role()
                    {
                        Id="guest",
                        RoleName="Guest User",
                        Description="Guest User",
                    },
                    new Model.AuthenModels.Role()
                    {
                        Id="student",
                        RoleName="Student User",
                        Description="Student User",
                    }
                }
            );
            context.SaveChanges();
        }
    }
}