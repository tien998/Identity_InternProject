using IdentityServices.Authentication;
using IdentityServices.Authentication.DTO;
using UserServices;

public static class StudentManagement
{
    public static void AddStudentManagement(this WebApplication app)
    {
        app.Map("/student", app =>
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                // 'index' to Pagination, 'take' is number of item that need to take
                endpoints.MapGet("/getAll/{index}/{take}", (int index, int take, UserServices.UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            httpContext.Response.StatusCode = 200;
                            httpContext.Response.WriteAsJsonAsync(userManipulator.GetStudents(index, take));
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/register", (StudentRegister_DTO user, HttpContext httpContext, AuthenManager authenManager) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            authenManager.Register_Student(user, httpContext);
                            httpContext.Response.StatusCode = 200;
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/edit", (StudentRs_DTO student, UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            userManipulator.EditStudent(student);
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
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
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