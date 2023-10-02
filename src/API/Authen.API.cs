using IdentityServices.Authentication.DTO;
using IdentityServices.Authentication;

public static class AuthenAPI
{
    public static void AddAuthenAPI(this WebApplication app)
    {
        app.MapPost("/register", (UserDTO user, HttpContext httpContext, AuthenManager authenManager) =>
        {
            authenManager.Register_Guest(user.UserName!, user.Password!, httpContext);
        });
        app.MapPost("/registerStudentUser", (StudentUserDTO user, HttpContext httpContext, AuthenManager authenManager) =>
        {
            string JwtBearer = httpContext.Request.Headers["Authorization"].ToString();
            string jwt = JwtBearer.Split(" ")[1];
            // Authorization function to check the role of user
            try
            {
                bool isValid = authenManager.AuthorizeChecking(jwt, RolesList.sa);
                if (isValid)
                {
                    authenManager.Register_Student(user, httpContext);
                }
                else
                {
                    httpContext.Response.StatusCode = 401;
                }
            }
            catch
            {
                httpContext.Response.StatusCode = 401;
            }
        });
        app.MapPost("/signin", (UserDTO user, HttpContext httpContext, AuthenManager authenManager) =>
        {
            try
            {
                authenManager.SignIn(user.UserName!, user.Password!, httpContext);
            }
            catch
            {
                httpContext.Response.StatusCode = 401;
            }
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

        // This API is a test of authorize 
        app.MapGet("/author/{jwt}", (string jwt, AuthenManager authenManager, HttpContext httpContext) =>
        {
            // if Authorize success! Allow excute request. 
            // otherwise! Return 401
            bool isAuthor = authenManager.AuthorizeChecking(jwt, RolesList.sa);
            return isAuthor;
        });
    }
}