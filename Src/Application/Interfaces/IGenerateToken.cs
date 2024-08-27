using System.Security.Claims;

namespace Application.Interfaces;

public interface IGenerateToken
{
    string GenerateToken(string userId, string email, string role);
}