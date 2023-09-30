namespace Model.AuthenModels;

public class User
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Hash_password { get; set; }
    public string? Salt { get; set; }
    public string? FirstName {get; set; }
    public string? LastName {get; set; }
    public string? Gender { get; set; }
    public string? Email {get; set; }
    public string? Telephone {get; set; }
    public string? Address {get; set; }
    public string? Parents {get; set; }

    public List<Role_User>? Role_User { get; set; }
    public List<ResetPassToken>? ResetPassTokens { get; set; }
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

public class ResetPassToken
{

    public int User_Id { get; set;}
    public string? Guid { get; set;}
    public DateTime ExpiryTime { get; set;}
    public User? User {get; set;}
    public ResetPassToken(){}
    public ResetPassToken(int user_Id, string? guid, DateTime expiryTime)
    {
        User_Id = user_Id;
        Guid = guid;
        ExpiryTime = expiryTime;
    }
}