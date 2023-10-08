using IdentityServices.Authentication;
using IdentityServices.Authentication.DTO;

public static class StudentManagement
{
    public static void AddStudentManagement(this WebApplication app)
    {
        // 'index' to Pagination, 'take' is number of item that need to take
        app.MapGet("/Students/{index}/{take}", (int index, int take, UserServices.UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
        {
            string jwtBearer = httpContext.Request.Headers["Authorization"].ToString();
            string jwt = jwtBearer.Split(" ")[1];

            bool isValid = authenManager.AuthorizeChecking(jwt, RoleConventions.sa, httpContext);
            if (isValid)
            {
                httpContext.Response.WriteAsJsonAsync(userManipulator.GetStudents(index, take));
            }
            else
            {
                httpContext.Response.StatusCode = 401;
            }
        });

        app.MapPost("/registerStudent", (StudentRq_DTO user, HttpContext httpContext, AuthenManager authenManager) =>
        {
            string JwtBearer = httpContext.Request.Headers["Authorization"].ToString();
            string jwt = JwtBearer.Split(" ")[1];
            // Authorization function to check the role of user
            bool isValid = authenManager.AuthorizeChecking(jwt, RoleConventions.sa, httpContext);
            if (isValid)
            {
                authenManager.Register_Student(user, httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 401;
            }
        });
    }
}