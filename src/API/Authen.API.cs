using IdentityServices.Authentication.DTO;
using IdentityServices.Authentication;

public static class AuthenAPI
{
    public static void AddAuthenAPI(this WebApplication app)
    {
        app.MapPost("/register", (UserDTO user, HttpContext httpContext, AuthenManager authenManager) =>
        {
            authenManager.Register(user.UserName!, user.Password!, httpContext);
        });
        app.MapPost("/signin", (UserDTO user, HttpContext httpContext, AuthenManager authenManager) =>
        {
            authenManager.SignIn(user.UserName!, user.Password!, httpContext);
        });
        app.MapPost("/sendEmailResetPassword", (EmailDTO email, AuthenManager authenManager, HttpContext httpContext) =>
        {
            try
            {
                authenManager.SendEmailResetPassword(email.EmailAddress!);
                httpContext.Response.WriteAsync("The URL to confirm had been send! Please check you Email!");
            }
            catch
            {
                httpContext.Response.WriteAsync("Can't find the Email! Please check the correct of Email Address!");
            }
        });
        app.MapPost("/ResetPass", (ResetPassDTO resetPass, AuthenManager authenManager, HttpContext httpContext) =>
        {
            try
            {
                authenManager.ValidateAndResetPassword(resetPass.GUID!, resetPass.UserID, resetPass.Password!, httpContext);
            }
            catch
            {
                httpContext.Response.WriteAsync("Can't find the Email! Please check the correct of Email Address!");
            }
        });
    }
}