using IdentityServices.Authentication.DTO;
using Model.AuthenModels;

namespace UserServices;

public class UserManipulator
{
    AuthenDb _authenDb;

    public StudentRsDTO[] GetStudents(int index, int take)
    {
        var users = (from usr in _authenDb.User
                     join ru in _authenDb.Role_User on usr.Id equals ru.User_Id
                     where ru.Role_Id == RoleConventions.student
                     select usr).OrderBy(e => e.Id).Skip(index * take).Take(take).ToArray();
        var students = new StudentRsDTO[users.Length];
        for (int i = 0; i < users.Length; i++)
        {
            students[i] = new StudentRsDTO(users[i]);
        }
        return students;
    }

    public User GetUser(int id)
    {
        var user = (from usr in _authenDb.User
                     where usr.Id == id
                     select usr).FirstOrDefault();
        return user!;
    }

    public void EditStudent(StudentRsDTO student)
    {
        var users = (from usr in _authenDb.User
                     where usr.Id.ToString() == student.Id
                     select usr).FirstOrDefault();
        DTO_Transaction.Transact(users!, student );
        _authenDb.SaveChanges();
    }

    public TeacherRsDTO[] GetTeachers(int index, int take)
    {
        var users = (from usr in _authenDb.User
                     join ru in _authenDb.Role_User on usr.Id equals ru.User_Id
                     where ru.Role_Id == RoleConventions.teacher
                     select usr).OrderBy(e => e.Id).Skip(index * take).Take(take).ToArray();
        var teacher = new TeacherRsDTO[users.Length];
        for (int i = 0; i < users.Length; i++)
        {
            teacher[i] = new TeacherRsDTO(users[i]);
        }
        return teacher;
    }

    public TeacherDropdown_DTO[] GetTeachers_DropdownDTO()
    {
        var users = (from usr in _authenDb.User
                     join ru in _authenDb.Role_User on usr.Id equals ru.User_Id
                     where ru.Role_Id == RoleConventions.teacher
                     select new{usr.Id, usr.FirstName, usr.LastName}).ToArray();
        var teacher = new TeacherDropdown_DTO[users.Length];
        for (int i = 0; i < users.Length; i++)
        {
            teacher[i] = new TeacherDropdown_DTO()
            {
                Id = users[i].Id,
                FirstName = users[i].FirstName,
                LastName = users[i].LastName
            };
        }
        return teacher;
    }

    public void EditTeacher(TeacherRsDTO teacher)
    {
        var users = (from usr in _authenDb.User
                     where usr.Id.ToString() == teacher.Id
                     select usr).FirstOrDefault();
        DTO_Transaction.Transact(users!, teacher);
        _authenDb.SaveChanges();
    }

    public void DeleteUser(int id)
    {
        var ru = (from rus in _authenDb.Role_User
                    where rus.User_Id == id
                    select rus).ToArray();
        var user = (from User in _authenDb.User
                        where User.Id == id
                        select User).FirstOrDefault();
        _authenDb.RemoveRange(ru);
        _authenDb.Remove(user!);
        _authenDb.SaveChanges();
    }

    public UserManipulator(AuthenDb authenDb)
    {
        _authenDb = authenDb;
    }
}