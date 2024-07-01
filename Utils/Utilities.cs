using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebForm.Models;

namespace WebForm.Utils;

public class Utilities
{
    private readonly IConfiguration _configuration;
    public Utilities(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string EncryptSHA256(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new ArgumentNullException(nameof(text), "The text to encrypt cannot be null or empty.");
        }
        using (SHA256 sha256hash = SHA256.Create())
        {
            byte[] bytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(text));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
    
    public string JwtGenerateToken(User user)
    {
        var userClaims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var jwtConfig = new JwtSecurityToken(
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        
        var token = new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        return token;
    }
}