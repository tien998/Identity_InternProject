using IdentityServices.Authentication.DTO;
using IdentityServices.Authentication;

public static class AuthenAPI
{
    public static void AddAuthenAPI(this WebApplication app)
    {
        app.MapPost("/register", (User_AuthenDTO user, HttpContext httpContext, AuthenManager AuthenManager) =>
        {
            AuthenManager.Register_Guest(user.UserName!, user.Password!, httpContext);
        });

        app.MapPost("/signin", (User_AuthenDTO user, HttpContext httpContext, AuthenManager AuthenManager) =>
        {
            try
            {
                AuthenManager.SignIn(user.UserName!, user.Password!, httpContext);
            }
            catch
            {
                httpContext.Response.StatusCode = 401;
            }
        });

        app.MapPost("/sendEmailResetPassword", (EmailDTO email, AuthenManager AuthenManager, HttpContext httpContext) =>
        {
            try
            {
                AuthenManager.SendEmailResetPassword(email.EmailAddress!);
                httpContext.Response.WriteAsync("The URL to confirm had been send! Please check you Email!");
            }
            catch
            {
                httpContext.Response.WriteAsync("Can't find the Email! Please check the correct of Email Address!");
            }
        });

        app.MapPost("/ResetPass", (ResetPassDTO resetPass, AuthenManager AuthenManager, HttpContext httpContext) =>
        {
            try
            {
                AuthenManager.ValidateAndResetPassword(resetPass.GUID!, resetPass.UserID, resetPass.Password!, httpContext);
            }
            catch
            {
                httpContext.Response.WriteAsync("Can't find the Email! Please check the correct of Email Address!");
            }
        });

        // This API is a check the authorization of role
        app.MapGet("/authorize/{role}", (string role, AuthenManager AuthenManager, HttpContext httpContext) =>
        {
            // if Authorize success! Allow excute request. 
            // otherwise! Return 401
            try
            {
                bool isAuthor = AuthenManager.IsAuthorize(httpContext, role);
                httpContext.Response.StatusCode = 200;
                return isAuthor;
            }
            catch
            {
                httpContext.Response.StatusCode = 401;
                return false;
            }
        });
    }
}