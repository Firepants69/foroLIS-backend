using foroLIS_backend.DTOs;

namespace foroLIS_backend.Services
{
    public interface IUserService
    {
        Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto request, GoogleUserDto googleUser);
        Task<CurrentUserResponseDto> GetCurrentUserAsync();
        Task<UserResponseDto> GoogleLoginAsync(UserRegisterRequestDto request);
        Task<UserResponseDto> GetByIdAsync(string id);
        Task<UserResponseDto> UpdateAsync(string id, UpdateUserDto request);
        Task DeleteAsync(string id);
        Task<RevokeRefreshTokenResponseDto> RevokeRefreshToken(RefreshTokenRequestDto refreshTokenRemoveRequest);
        Task<CurrentUserResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task<UserResponseDto> LoginAsync(UserLoginRequestDto request);
    }
}
