
using IdentityServices.Authentication;
using IdentityServices.Authentication.DTO;

namespace IdentityServices;

public class Authorize_SA : IMiddleware
{
    AuthenManager _authenManager;
    public Authorize_SA(AuthenManager authenManager)
    {
        _authenManager = authenManager;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        try
        {
            bool isAuthor = AuthenManager.IsAuthorize(context, RoleConventions.sa!);
            if (isAuthor)
            {
                await next(context);
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorize!");
            }
        }
        catch
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorize!");
        }
    }
}
