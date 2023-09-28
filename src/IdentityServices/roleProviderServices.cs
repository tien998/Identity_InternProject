using Model.AuthenModels;

namespace IdentityServices;

public static class RoleProvider
{
    public static string AddRoleSA(User user, AuthenDb context)
    {
        Role_User? role_user = new()
        {
            User_Id = user.Id,
            Role_Id = "sa"
        };
        context.Role_User.Add(role_user);
        role_user = null;
        context.SaveChanges();
        return "sa";
    }

    public static string AddRoleGuest(User user, AuthenDb context)
    {
        Role_User? role_user = new()
        {
            User_Id = user.Id,
            Role_Id = "guest"
        };
        context.Role_User.Add(role_user);
        role_user = null;
        context.SaveChanges();
        return "guest";
    }

    public static string AddRoleStudent(User user, AuthenDb context)
    {
        Role_User? role_user = new()
        {
            User_Id = user.Id,
            Role_Id = "student"
        };
        context.Role_User.Add(role_user);
        role_user = null;
        context.SaveChanges();
        return "student";
    }
}