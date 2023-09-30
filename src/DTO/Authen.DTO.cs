namespace IdentityServices.Authentication.DTO;

public class UserDTO
{
    public string? UserName { get; set;}
    public string? Password { get; set;}
}

public class EmailDTO
{
    public string? EmailAddress { get; set;}
}

public class ResetPassDTO
{
    public string? GUID {get; set;}
    public int UserID { get; set;}
    public string? Email { get; set;}
    public string? Password {get; set;}
}