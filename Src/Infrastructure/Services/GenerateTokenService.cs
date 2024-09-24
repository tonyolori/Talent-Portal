using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class GenerateTokenService(string key, string issuer, string audience) : IGenerateToken
    {
        private readonly string _key = key;
        private readonly string _issuer = issuer;
        private readonly string _audience = audience;

        public string GenerateToken(string userId, string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public string GetEmailFromToken(ClaimsPrincipal user)
        {
            var emailClaim = user.FindFirst(ClaimTypes.Email)?.Value;
            if (emailClaim == null)
            {
                throw new UnauthorizedAccessException("Email not found in token.");
            }

            return emailClaim;
        }
        
        public string GetIdFromToken(ClaimsPrincipal user)  
        {  
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;  
            if (idClaim == null)  
            {  
                throw new UnauthorizedAccessException("User ID not found in token.");  
            }  

            return idClaim;  
        }  
    }
}