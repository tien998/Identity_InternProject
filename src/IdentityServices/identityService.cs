using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServices;

public static class Identity
{
    static int iteration = 1000;
    static int saltSize = 32;
    static int keySize = 32;
    static HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA1;
    
    public static byte[] CreateSalt()
    {
        byte[] salt = new byte[saltSize];
        var generator = RandomNumberGenerator.Create();
        generator.GetBytes(salt);
        generator.Dispose();
        return salt;
    }
    public static byte[] GetIV(string? hashPassword)
    {
        string[] pwSplit = hashPassword!.Split(".");
        string IV = pwSplit[0];
        return Convert.FromBase64String(IV);
    }
    public static byte[] CreateKey(string? password, byte[] saltByte)
    {
        Rfc2898DeriveBytes deriveBytes = new(password!, saltByte, iteration, hashAlgorithm);
        var key = deriveBytes.GetBytes(keySize);
        deriveBytes.Dispose();
        return key;
    }

    /// <summary>
    /// Construct Aes using "aesAlg = Aes.Create();" before use this function. After, dispose Aes
    /// </summary>
    public static string EncryptPassword(string password, byte[] key, Aes aesAlg, byte[] IV)
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
    public static string CreateJWT(string userName, string secretkey)
    {
        // Create JWT
        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretkey));
        Claim[] claims = new[] {
            new Claim(JwtRegisteredClaimNames.Name, userName),
        };
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        return jwtSecurityTokenHandler.WriteToken(token);
    }

    public static string CreateJWT(string userName, string role, string secretkey)
    {
        // Create JWT
        SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(secretkey));
        Claim[] claims = new[] {
            new Claim(JwtRegisteredClaimNames.Name, userName),
            new Claim(ClaimTypes.Role, role),
        };
        SecurityTokenDescriptor securityTokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
        };
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();
        var token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        return jwtSecurityTokenHandler.WriteToken(token);
    }

    public static ClaimsPrincipal ReadJWT(string jwt, string secretkey)
    {
        SymmetricSecurityKey securityKey = new(Encoding.ASCII.GetBytes(secretkey));
        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateActor = false,
            IssuerSigningKey = securityKey,
        };
        return tokenHandler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken validatedToken);
    }
}