using System.Security.Cryptography;
using Model.AuthenModels;

namespace IdentityServices.Authentication;

public class AuthenManager
{
    AuthenDb? _authenDb;
    string? _JwtSecreatKey;
    Aes? aesAlg;

    public void Register(string userName, string password, HttpContext httpContext)
    {
        CreateHashPassPrinciple(password, out string hashPassword, out string saltBase64);
        User user = new()
        {
            UserName = userName,
            Hash_password = hashPassword,
            Salt = saltBase64,
        };
        var userEntry = _authenDb!.User.Add(user);
        var user_inDB = userEntry.Entity;

        _authenDb!.SaveChanges();
        // string roleName = RoleProvider.AddRoleGuest(user_inDB, _authenDb);
        string jwt = Identity.CreateJWT(user_inDB.UserName!, _JwtSecreatKey!);
        httpContext.Response.Headers.Add("Authorization", $"Bearer {jwt}");
        httpContext.Response.StatusCode = 200;
    }

    public void SignIn(string userName, string password, HttpContext httpContext)
    {
        var user = _authenDb!.User.Where(e => e.UserName == userName).FirstOrDefault();
        string? salt = user!.Salt;
        byte[] saltKeyByte = Convert.FromBase64String(salt!);
        byte[] IV = Identity.GetIV(user.Hash_password);
        aesAlg = Aes.Create();
        var hashPassword = Identity.EncryptPassword(password, saltKeyByte, aesAlg, IV);
        aesAlg.Dispose();
        // Compare hashPassword
        if (hashPassword == user.Hash_password)
        {
            // Đăng nhập thành công
            try
            {
                var user_role = from User in _authenDb.User
                                join Role_User in _authenDb.Role_User
                                on User.Id equals Role_User.User_Id
                                where User.UserName == userName
                                select new
                                {
                                    userName = User.UserName,
                                    roleID = Role_User.Role_Id,
                                };
                var ur = user_role.FirstOrDefault();
                string jwt = Identity.CreateJWT(ur!.userName, ur.roleID, _JwtSecreatKey!);
                httpContext.Response.Headers.Add("Authorization", $"Bearer {jwt}");
            }
            catch
            {
                string jwt = Identity.CreateJWT(user!.UserName!, _JwtSecreatKey!);
                httpContext.Response.Headers.Add("Authorization", $"Bearer {jwt}");
            }
            httpContext.Response.StatusCode = 200;
        }
        else
        {
            httpContext.Response.StatusCode = 401;
        }
    }

    public void SendEmailResetPassword(string emailTo)
    {
        var user = (from User in _authenDb!.User
                    where User.Email == emailTo
                    select User)
               .FirstOrDefault();
        int userID = user!.Id;
        string userName = user.UserName!;
        string guid = Guid.NewGuid().ToString();
        DateTime expiry = DateTime.UtcNow.AddMinutes(15);
        _authenDb.ResetPassToken.Add(new ResetPassToken(userID, guid, expiry));
        _authenDb.SaveChanges();
        SendMailService? sendMail = new();
        sendMail.SendEmailPasswordReset(emailTo, guid, userID, userName);
        user = null;
        sendMail = null;
    }

    public void ValidateAndResetPassword(string guid, int userID, string password, HttpContext httpContext)
    {
        var resetToken = (from ResetPassToken in _authenDb!.ResetPassToken
                          where ResetPassToken.User_Id == userID && ResetPassToken.Guid == guid
                          select ResetPassToken).FirstOrDefault();
        if (resetToken!.Guid == guid && resetToken.User_Id == userID)
        {
            if (resetToken.ExpiryTime < DateTime.Now)
            {
                // Create new hash password
                var user = (from User in _authenDb.User
                            where User.Id == userID
                            select User).FirstOrDefault();
                CreateHashPassPrinciple(password, out string hashPassword, out string saltBase64);
                user!.Hash_password = hashPassword;
                user.Salt = saltBase64;
                _authenDb.SaveChanges();
                resetToken = null;
            }
            else
            {
                httpContext.Response.StatusCode = 401;
            }
        }
    }

    void CreateHashPassPrinciple(string password, out string hashPassword, out string saltBase64)
    {
        // Salt
        byte[] saltByte = Identity.CreateSalt();
        // Key
        byte[] saltKey = Identity.CreateKey(password, saltByte);
        aesAlg = Aes.Create();
        // IV
        aesAlg.GenerateIV();
        // Hash pw
        hashPassword = Identity.EncryptPassword(password, saltKey, aesAlg, aesAlg.IV);
        aesAlg.Dispose();
        saltBase64 = Convert.ToBase64String(saltKey).ToString();
    }

    public AuthenManager(AuthenDb authenDb)
    {
        _authenDb = authenDb;
        ConfigurationBuilder? confBuilder = new();
        confBuilder.AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json");
        var root = confBuilder.Build();
        _JwtSecreatKey = root.GetSection("JWTSecurityKey").Value!;
        confBuilder = null;
        root = null;
    }
}