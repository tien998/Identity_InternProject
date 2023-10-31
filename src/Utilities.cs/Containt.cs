using System.Security.Claims;

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
