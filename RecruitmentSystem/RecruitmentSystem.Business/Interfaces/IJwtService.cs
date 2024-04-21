using System.Security.Claims;

namespace RecruitmentSystem.Business.Interfaces;

public interface IJwtService
{
    string CreateAccessToken(string userName, string userId, IEnumerable<string> userRoles);
    string CreateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}