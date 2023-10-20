using Model.AuthenModels;

namespace IdentityServices.Authentication.DTO;

public class TeacherDropdown_DTO
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public TeacherDropdown_DTO() { }
}