using IdentityServices.Authentication;
using Microsoft.EntityFrameworkCore;
using Model.AuthenModels;

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
                    new Role()
                    {
                        Id="sa",
                        RoleName="Supper Admin",
                        Description="Ultimate Right Admin",
                    },
                    new Role()
                    {
                        Id="teacher",
                        RoleName="Teaher User",
                        Description="Teacher User",
                    },
                    new Role()
                    {
                        Id="student",
                        RoleName="Student User",
                        Description="Student User",
                    }
                }
            );
            AuthenManager? authenManager = new(context);
            authenManager.CreateHashPassPrinciple("123456", out string hashPassword, out string saltBase64);
            var userEntity = context.User.Add(
                new User()
                {
                    UserName = "sa",
                    Hash_password = hashPassword,
                    Salt = saltBase64,
                }
            );
            authenManager = null;
            context.SaveChanges();
            var user = userEntity.Entity;
            context.Role_User.Add(
                new Role_User()
                {
                    User_Id = user.Id,
                    Role_Id = "sa"
                }
            );
            context.SaveChanges();
        }
    }
}