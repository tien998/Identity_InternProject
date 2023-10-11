using IdentityServices.Authentication;
using IdentityServices.Authentication.DTO;
using UserServices;

public static class TeacherManagement
{
    public static void AddTeacherManagement(this WebApplication app)
    {
        app.Map("/teacher", app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/GetAll/{index}/{take}", (int index, int take, UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        string jwtBearer = httpContext.Request.Headers["Authorization"].ToString();
                        string jwt = jwtBearer.Split(" ")[1];

                        bool isValid = authenManager.AuthorizeChecking(jwt, RoleConventions.sa, httpContext);
                        if (isValid)
                        {
                            httpContext.Response.WriteAsJsonAsync(userManipulator.GetTeachers(index, take));
                            httpContext.Response.StatusCode = 200;
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/register", (TeacherRegister_DTO user, HttpContext httpContext, AuthenManager authenManager) =>
                {
                    try
                    {
                        string JwtBearer = httpContext.Request.Headers["Authorization"].ToString();
                        string jwt = JwtBearer.Split(" ")[1];
                        // Authorization function to check the role of user
                        bool isValid = authenManager.AuthorizeChecking(jwt, RoleConventions.sa, httpContext);
                        if (isValid)
                        {
                            authenManager.Register_Teacher(user, httpContext);
                            httpContext.Response.StatusCode = 200;
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/edit", (TeacherRs_DTO teacherRs_DTO, UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        string JwtBearer = httpContext.Request.Headers["Authorization"].ToString();
                        string jwt = JwtBearer.Split(" ")[1];
                        // Authorization function to check the role of user
                        bool isValid = authenManager.AuthorizeChecking(jwt, RoleConventions.sa, httpContext);
                        if (isValid)
                        {
                            userManipulator.EditTeacher(teacherRs_DTO);
                            httpContext.Response.StatusCode = 200;
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapGet("/delete/{userID}", (int userID, UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        string JwtBearer = httpContext.Request.Headers["Authorization"].ToString();
                        string jwt = JwtBearer.Split(" ")[1];
                        // Authorization function to check the role of user
                        bool isValid = authenManager.AuthorizeChecking(jwt, RoleConventions.sa, httpContext);
                        if (isValid)
                        {
                            userManipulator.DeleteUser(userID);
                            httpContext.Response.StatusCode = 200;
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });
            });
        });
    }
}