using IdentityServices.Authentication.DTO;
using IdentityServices.Authentication;

public static class AuthenAPI
{
    public static void AddAuthenAPI(this WebApplication app)
    {
        app.MapPost("/register", (UserDTO user, HttpContext httpContext, AuthenManager authenManager) => {
            authenManager.Register(user.UserName!, user.Password!, httpContext);
        });
        app.MapPost("/signin", (UserDTO user, HttpContext httpContext, AuthenManager authenManager) => {
            authenManager.SignIn(user.UserName!, user.Password!, httpContext);
        });
    }
}