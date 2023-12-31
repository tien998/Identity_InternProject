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

                endpoints.MapGet("/getUser/{id}", (int id, UserServices.UserManipulator userManipulator, AuthenManager authenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            httpContext.Response.StatusCode = 200;
                            TeacherRsDTO teacher = new(userManipulator.GetUser(id));
                            httpContext.Response.WriteAsJsonAsync(teacher);
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapGet("/GetAll/dropdown", (UserManipulator userManipulator, AuthenManager AuthenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            httpContext.Response.StatusCode = 200;
                            httpContext.Response.WriteAsJsonAsync(userManipulator.GetTeachers_DropdownDTO());
                        }
                    }
                    catch
                    {
                        httpContext.Response.StatusCode = 401;
                    }
                });

                endpoints.MapPost("/register", (TeacherRegisterDTO user, HttpContext httpContext, AuthenManager AuthenManager) =>
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

                endpoints.MapPost("/edit", (TeacherRsDTO teacherRsDTO, UserManipulator userManipulator, AuthenManager AuthenManager, HttpContext httpContext) =>
                {
                    try
                    {
                        bool isValid = AuthenManager.IsAuthorize(httpContext, RoleConventions.sa);
                        if (isValid)
                        {
                            userManipulator.EditTeacher(teacherRsDTO);
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