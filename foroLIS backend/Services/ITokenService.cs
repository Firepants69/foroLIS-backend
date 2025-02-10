using foroLIS_backend.Models;

namespace foroLIS_backend.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(Users user);
        string GenerateRefreshToken();
    }
}
