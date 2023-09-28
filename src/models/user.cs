namespace Model.AuthenModels;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Hash_password { get; set; }
    public string? DisplayUserName { get; set; }
    public string? Language_Id { get; set; }
    public string? Salt { get; set; }
    public User_language? User_Language { get; set; }
    public List<Role_User>? Role_User { get; set; }
}

public class User_language
{
    public string? Id { get; set; }
    public string? Language { get; set; }
    public List<User>? User { get; set; }
}

public class Role
{
    public string? Id { get; set; }
    public string? RoleName { get; set; }
    public string? Description { get; set; }
    public List<Role_User>? Role_User { get; set; }
    public List<Role_Permission>? Role_Permissions{ get; set; }

}

public class Role_User
{
    public int User_Id { get; set; }
    public User? User {get; set;}
    public string? Role_Id { get; set; }
    public Role? Role {get; set;}
}

public class Permission
{
    public int Id { get; set; }
    public string? Permission_Name { get; set; }
    public string? Description { get; set; }
    public List<Role_Permission>? Role_Permissions{ get; set; }
}

public class Role_Permission
{
    public string? Role_Id { get; set; }
    public Role? Role {get; set;}
    public int Permission_Id { get; set; }
    public Permission? Permission {get; set;}
}