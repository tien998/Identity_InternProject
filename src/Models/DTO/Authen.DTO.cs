using System.Security.Claims;
using System.Text.Json;
using Model.AuthenModels;

namespace IdentityServices.Authentication.DTO;

public class UserAuthenDTO
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
}

public class StudentRegisterDTO : UserBaseDTO, IStudentUser
{
    public string? Password { get; set; }
    public string? Parents { get; set; }
    public string? Avatar { get; set; }
}

public class StudentRsDTO : UserBaseDTO, IStudentUser
{
    public string? Id { get; set; }
    public string? Parents { get; set; }
    public StudentRsDTO() {}
    public StudentRsDTO(User user) {
        Id = user.Id.ToString();
        FirstName = user.FirstName;
        LastName = user.LastName;
        DateOfBirth = JsonSerializer.Serialize(user.DateOfBirth);
        Gender = user.Gender;
        Email = user.Email;
        Telephone = user.Telephone;
        Address = user.Address;
        Parents = user.Parents;
    }
}

public class TeacherRegisterDTO : UserBaseDTO, ITeacherUser
{
    public string? TaxIdentificationNumber { get; set; }
    public string? MajorSubject { get; set; }
    public string? MinorSubject { get; set; }
    public string? Password { get; set; }
}

public class TeacherRsDTO : UserBaseDTO, ITeacherUser
{
    public string? Id { get; set; }
    public string? TaxIdentificationNumber { get; set; }
    public string? MajorSubject { get; set; }
    public string? MinorSubject { get; set; }
    public TeacherRsDTO() {}
    public TeacherRsDTO(User user) {
        Id = user.Id.ToString();
        FirstName = user.FirstName;
        LastName = user.LastName;
        DateOfBirth = JsonSerializer.Serialize(user.DateOfBirth);
        Gender = user.Gender;
        Email = user.Email;
        Telephone = user.Telephone;
        Address = user.Address;
        MajorSubject = user.MajorSubject;
        MinorSubject = user.MinorSubject;
    }
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
