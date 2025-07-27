using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SupplierManagement.Models.Api;
using SupplierManagement.Services.Interfaces;

namespace SupplierManagement.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TokenDto GenerateToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatIs32CharactersLong!";
            var issuer = jwtSettings["Issuer"] ?? "SupplierManagementAPI";
            var audience = jwtSettings["Audience"] ?? "SupplierManagementAPI";
            var expirationHours = int.Parse(jwtSettings["ExpirationHours"] ?? "24");

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier, username),
                    new Claim("username", username)
                }),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new TokenDto
            {
                Token = tokenString,
                Expiration = tokenDescriptor.Expires.Value
            };
        }

        public bool ValidateCredentials(string username, string password)
        {
            // For demo purposes, using simple hardcoded credentials
            // In production, this would validate against a user store/database
            var validCredentials = new Dictionary<string, string>
            {
                { "admin", "password123" },
                { "user", "userpass" },
                { "api_user", "api_secret" }
            };

            return validCredentials.ContainsKey(username) && validCredentials[username] == password;
        }
    }
}
