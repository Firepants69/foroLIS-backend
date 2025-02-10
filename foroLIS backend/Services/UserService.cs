using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using foroLIS_backend.DTOs;
using foroLIS_backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace foroLIS_backend.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<Users> _userManager;
        private readonly ILogger<UserService> _logger;
        private readonly GoogleService _googleService;

        public UserService(
            ITokenService tokenService
            , ICurrentUserService currentUserService
            , UserManager<Users> userManager
            , ILogger<UserService> logger
            ,GoogleService googleService)
        {
            _tokenService = tokenService;
            _currentUserService = currentUserService;
            _userManager = userManager;
            _logger = logger;
            _googleService = googleService;
        }

        
        public async Task<UserResponseDto> RegisterAsync(UserRegisterRequestDto request, GoogleUserDto googleUser)
        {
            
            _logger.LogInformation("Registering user");
            var existingUser = await _userManager.FindByEmailAsync(googleUser.email);
            if (existingUser != null)
            {
                _logger.LogError("Email already exists");
                throw new Exception("Email already exists");
            }

            var newUser = GoogleUserDto.GoogleUserDtoToUsers(googleUser, request);
            newUser.UserName = googleUser.email;

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to create user: {errors}", errors);
                throw new Exception($"Failed to create user: {errors}");
            }
            _logger.LogInformation("User created successfully");
            await _tokenService.GenerateToken(newUser);
            newUser.CreateAt = DateTime.Now;
            newUser.UpdateAt = DateTime.Now;
            newUser.Lastlogin = DateTime.Now;
            
            var userId = newUser.Id;

            var response = Users.UsersToUsersResponseDto(newUser);
            
            return response;

        }

       

        public async Task<UserResponseDto> LoginAsync(UserLoginRequestDto request)
        {
            if(request == null)
            {
                _logger.LogError("Login request is null");
                throw new ArgumentNullException(nameof(request));
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                _logger.LogError("Invalid email");
                throw new Exception("Invalid email");
            }
            var token = await _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            using var sha256 = SHA256.Create();
            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshToken));
            user.RefreshToken = Convert.ToBase64String(refreshTokenHash);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(2);

            user.Lastlogin = DateTime.Now;
            user.ArchitectureOS = request.ArchitectureOS;
            user.FamilyOS = request.FamilyOS;
            user.ArchitectureOS = request.ArchitectureOS;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to update user: {errors}", errors);
                throw new Exception($"Failed to update user: {errors}");
            }
            var userResponse = Users.UsersToUsersResponseDto(user);
            userResponse.AccessToken = token;
            userResponse.RefreshToken = refreshToken;
            userResponse.ArchitectureOS = request.ArchitectureOS;
            userResponse.VersionOS = request.VersionOS;
            userResponse.FamilyOS = request.FamilyOS;
            return userResponse;
        }

        public async Task<UserResponseDto> GoogleLoginAsync(UserRegisterRequestDto request)
        {
            GoogleUserDto googleUserDto = await _googleService.GetUserByToken(request.GoogleToken);
            var user = await _userManager.FindByEmailAsync(googleUserDto.email);

            if (user == null)
            {
                await RegisterAsync(request, googleUserDto);
             
            }
                UserLoginRequestDto loginRequestDto = new UserLoginRequestDto()
                { Email = googleUserDto.email };
                return await LoginAsync(loginRequestDto);
        }

        public async Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserResponseDto> GetByIdAsync(string id)
        {
            _logger.LogInformation("Getting user by id");
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }
            _logger.LogInformation("User found");

            return Users.UsersToUsersResponseDto(user);
        }

        public async Task<CurrentUserResponseDto> GetCurrentUserAsync()
        {
            var user = await _userManager.FindByIdAsync(_currentUserService.GetUserId());
            if (user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }
            return Users.UserToCurrentUserResponseDto(user);
        }


        public async Task<CurrentUserResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            _logger.LogInformation("RefreshToken");
            using var sha256 = SHA256.Create();
            var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(request.RefreshToken));
            var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
            if(user == null)
            {
                _logger.LogError("Invalid refresh token");
                throw new Exception("Invalid refresh token");
            }
            if(user.RefreshTokenExpiryTime > DateTime.Now)
            {
                _logger.LogWarning("RefreshToken expired for user ID: {UserId}", user.Id);
                throw new Exception("RefreshToken expired");
            }
            var newAccessToken = await _tokenService.GenerateToken(user);
            _logger.LogInformation("Access Token generated successfully");
            var currentUserResponse = Users.UserToCurrentUserResponseDto(user);
            currentUserResponse.AccessToken = newAccessToken;
            
            return currentUserResponse;
        }

        public async Task<RevokeRefreshTokenResponseDto> RevokeRefreshToken(RefreshTokenRequestDto refreshTokenRemoveRequest)
        {
            _logger.LogInformation("Revoking refresh token");
            try
            {
                using var sha256 = SHA256.Create();
                var refreshTokenHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(refreshTokenRemoveRequest.RefreshToken));
                var hashedRefreshToken = Convert.ToBase64String(refreshTokenHash);

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == hashedRefreshToken);
                if(user == null)
                {
                    _logger.LogError("Invalid refresh token");
                    throw new Exception("Invalid refresh token");
                }
                if(user.RefreshTokenExpiryTime < DateTime.Now)
                {
                    _logger.LogWarning("Refresh token expired for user ID: {UserId}", user.Id);
                    throw new Exception("Refresh token expired");
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiryTime = null;

                // Update user information in database
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    _logger.LogError("Failed to update user");
                    return new RevokeRefreshTokenResponseDto
                    {
                        Message = "Failed to revoke refresh token"
                    };
                }
                _logger.LogInformation("Refresh token revoked successfully");
                return new RevokeRefreshTokenResponseDto
                {
                    Message = "Refresh token revoked successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to revoke refresh token: {ex}", ex.Message);
                throw new Exception("Failed to revoke refresh token");
            }
        }

        public async Task<UserResponseDto> UpdateAsync(string id, UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                _logger.LogError("User not found");
                throw new Exception("User not found");
            }
            user.UpdateAt = DateTime.Now;
            user.Picture = request?.Picture ?? user.Picture;
            user.LastName = request?.LastName ?? user.LastName;
            user.FirstName = request?.FirstName ?? user.FirstName;
            user.Theme = request?.Theme ?? user.Theme;
            user.Language = request?.Language ?? user.Language;

            await _userManager.UpdateAsync(user);
            return Users.UsersToUsersResponseDto(user);
        }
    }
}
