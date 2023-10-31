namespace Model.AuthenModels;

public class UserBase
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Hash_password { get; set; }
    public string? Salt { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Address { get; set; }
}

public interface IStudentUser
{
    public string? Parents { get; set; }
}

public interface ITeacherUser
{
    public string? TaxIdentificationNumber { get; set; }
    public string? MajorSubject { get; set; }
    public string? MinorSubject { get; set; }
}

public class UserBaseDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Email { get; set; }
    public string? Telephone { get; set; }
    public string? Address { get; set; }
}