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

                endpoints.MapGet("/getUser/{id}", (int id, UserServices.UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            httpContext.Response.StatusCode = 200;
                            StudentRsDTO student = new(userManipulator.GetUser(id));
                            httpContext.Response.WriteAsJsonAsync(student);
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/register", (StudentRegisterDTO user, HttpContext httpContext, AuthenManager authenManager) =>
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

                endpoints.MapPost("/edit", (StudentRsDTO student, UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
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