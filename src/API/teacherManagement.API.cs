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
                endpoints.MapGet("/GetAll/{index}/{take}", (int index, int take, UserManipulator userManipulator, AuthenManager AuthenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            httpContext.Response.StatusCode = 200;
                            httpContext.Response.WriteAsJsonAsync(userManipulator.GetTeachers(index, take));
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/register", (TeacherRegister_DTO user, HttpContext httpContext, AuthenManager AuthenManager) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            AuthenManager.Register_Teacher(user, httpContext);
                            httpContext.Response.StatusCode = 200;
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/edit", (TeacherRs_DTO teacherRs_DTO, UserManipulator userManipulator, AuthenManager AuthenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
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

                endpoints.MapGet("/delete/{userID}", (int userID, UserManipulator userManipulator, AuthenManager AuthenManager, HttpContext httpContext) =>
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