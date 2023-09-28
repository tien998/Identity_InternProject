using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServices;

public static class Identity
{
    public static string CreateJWT(string userName, string role)
    {
        // Create JWT
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json");
        var root = configurationBuilder.Build();
        string secretkey = root.GetSection("JWTSecurityKey").Value!;
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

    public static ClaimsPrincipal ReadJWT(string jwt)
    {
        ConfigurationBuilder configurationBuilder = new();
        configurationBuilder.AddJsonFile(Directory.GetCurrentDirectory() + "/appsettings.json");
        var root = configurationBuilder.Build();
        string secretkey = root.GetSection("JWTSecurityKey").Value!;
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