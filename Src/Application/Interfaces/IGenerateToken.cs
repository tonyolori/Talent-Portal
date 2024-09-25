using System.Security.Claims;

namespace Application.Interfaces;

public interface IGenerateToken
{
    string GenerateToken(string userId, string email, string role);

    string GetEmailFromToken(ClaimsPrincipal user);

    string GetIdFromToken(ClaimsPrincipal user);
}