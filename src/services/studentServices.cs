using IdentityServices.Authentication.DTO;

namespace UserServices;

public class UserManipulator
{
    AuthenDb _authenDb;

    public StudentRs_DTO[] GetStudents(int index, int take)
    {
        var users = (from usr in _authenDb.User
                     join ru in _authenDb.Role_User on usr.Id equals ru.User_Id
                     where ru.Role_Id == RoleConventions.student
                     select usr).OrderBy(e => e.Id).Skip(index * take).Take(take).ToArray();
        var students = new StudentRs_DTO[users.Length];
        for (int i = 0; i < users.Length; i++)
        {
            students[i] = new StudentRs_DTO(users[i]);
        }
        return students;
    }
    public UserManipulator(AuthenDb authenDb)
    {
        _authenDb = authenDb;
    }
}