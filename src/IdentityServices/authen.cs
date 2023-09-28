using System.Security.Cryptography;
using Model.AuthenModels;

namespace IdentityServices.Authentication;

public class AuthenManager
{
    AuthenDb? _authenDb;
    int iteration = 1000;
    int saltSize = 32;
    int keySize = 32;
    Aes? aesAlg;
    HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA1;

    public void Register(string userName, string password, HttpContext httpContext)
    {
        // Salt
        byte[] saltByte = CreateSalt();
        // Key
        var SaltKey = CreateKey(password, saltByte);
        aesAlg = Aes.Create();
        // IV
        aesAlg.GenerateIV();
        // Hash pw
        var hashPassword = EncryptPassword(password, SaltKey, aesAlg.IV);
        aesAlg.Dispose();
        string? saltBase64 = Convert.ToBase64String(SaltKey).ToString();
        User user = new()
        {
            UserName = userName,
            Hash_password = hashPassword,
            Salt = saltBase64,
        };
        var userEntry = _authenDb!.User.Add(user);
        var user_inDB = userEntry.Entity;

        _authenDb!.SaveChanges();
        string roleName = RoleProvider.AddRoleGuest(user_inDB, _authenDb);
        string jwt = Identity.CreateJWT(user_inDB.UserName!, roleName);
        httpContext.Response.Headers.Add("Authorization", $"Bearer {jwt}");
        httpContext.Response.StatusCode = 200;
    }


    public void SignIn(string userName, string password, HttpContext httpContext)
    {
        var user = _authenDb!.User.Where(e => e.UserName == userName).FirstOrDefault();
        string? salt = user!.Salt;
        byte[] saltKeyByte = Convert.FromBase64String(salt!);
        byte[] IV = GetIV(user.Hash_password);
        aesAlg = Aes.Create();
        var hashPassword = EncryptPassword(password, saltKeyByte, IV);
        aesAlg.Dispose();
        // Compare hashPassword
        if (hashPassword == user.Hash_password)
        {
            // Đăng nhập thành công
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
            string jwt = Identity.CreateJWT(ur!.userName, ur.roleID);
            httpContext.Response.Headers.Add("Authorization", $"Bearer {jwt}");
            httpContext.Response.StatusCode = 200;
        }
        else
            httpContext.Response.StatusCode = 401;
    }
    byte[] CreateSalt()
    {
        byte[] salt = new byte[saltSize];
        var generator = RandomNumberGenerator.Create();
        generator.GetBytes(salt);
        generator.Dispose();
        return salt;
    }
    byte[] GetIV(string? hashPassword)
    {
        string[] pwSplit = hashPassword!.Split(".");
        string IV = pwSplit[0];
        return Convert.FromBase64String(IV);
    }
    byte[] CreateKey(string? password, byte[] saltByte)
    {
        Rfc2898DeriveBytes deriveBytes = new(password!, saltByte, iteration, hashAlgorithm);
        var key = deriveBytes.GetBytes(keySize);
        deriveBytes.Dispose();
        return key;
    }

    /// <summary>
    /// Construct Aes using "aesAlg = Aes.Create();" before use this function. After, dispose Aes
    /// </summary>
    string EncryptPassword(string password, byte[] key, byte[] IV)
    {
        string? IVstr;
        string? hashPasswordStr;
        using (MemoryStream stream = new())
        {
            using (CryptoStream encrypt = new(stream, aesAlg!.CreateEncryptor(key, IV), CryptoStreamMode.Write))
            {
                StreamWriter writer = new(encrypt);
                writer.Write(password);
                writer.Dispose();
            }
            hashPasswordStr = Convert.ToBase64String(stream.ToArray());
        }
        IVstr = Convert.ToBase64String(IV);
        return IVstr + "." + hashPasswordStr;
    }

    public AuthenManager(AuthenDb authenDb)
    {
        _authenDb = authenDb;
    }
}