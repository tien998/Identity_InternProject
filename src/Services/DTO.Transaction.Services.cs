using System.Text.Json;
using Model.AuthenModels;

namespace IdentityServices.Authentication.DTO;

public static class DTO_Transaction
{
    static public void Transact(User user, StudentRsDTO DTO)
    {
            user.FirstName = DTO.FirstName;
            user.LastName = DTO.LastName;
            user.DateOfBirth = JsonSerializer.Deserialize<DateTime>(DTO.DateOfBirth!);
            user.Gender = DTO.Gender;
            user.Email = DTO.Email;
            user.Telephone = DTO.Telephone;
            user.Address = DTO.Address;
            user.Parents = DTO.Parents;
    }

    static public void Transact(User user, TeacherRsDTO DTO)
    {
            user.FirstName = DTO.FirstName;
            user.LastName = DTO.LastName;
            user.DateOfBirth = JsonSerializer.Deserialize<DateTime>(DTO.DateOfBirth!);
            user.Gender = DTO.Gender;
            user.Email = DTO.Email;
            user.Telephone = DTO.Telephone;
            user.Address = DTO.Address;
            user.TaxIdentificationNumber = DTO.TaxIdentificationNumber;
            user.MajorSubject = DTO.MajorSubject;
            user.MinorSubject = DTO.MinorSubject;
    }
}