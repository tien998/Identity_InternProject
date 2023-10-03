using System.Security.Claims;

namespace IdentityServices.Authentication.DTO;

public class UserDTO
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class StudentUserDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Address { get; set; }
    public string? Parents { get; set; }
    public string? Password { get; set; }
    public string? Avatar { get; set; }
}

public class EmailDTO
{
    public string? EmailAddress { get; set; }
}

public class ResetPassDTO
{
    public string? GUID { get; set; }
    public int UserID { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public static class JwtPayloadConst
{
    public const string userID = "userID";
    public const string role = ClaimTypes.Role;
    public const string iss = "iss";
    public const string aud = "aud";
}

public static class RoleConventions
{
    public const string sa = "sa";
    public const string teacher = "teacher";
    public const string student = "student";

}