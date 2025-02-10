using System.ComponentModel.DataAnnotations;

namespace foroLIS_backend.DTOs
{
    public class UserRegisterRequestDto
    {
        public string GoogleToken { get; set; }
        public string ArchitectureOS { get; set; }
        public string? FamilyOS { get; set; }
        public string? VersionOS { get; set; }
        public string? Language { get; set; }
        public string Theme { get; set; }
    }

    public class UserResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? GoogleId { get; set; }
        public string Email { get; set; }
        public string? Picture { get; set; }
        public DateTime Lastlogin { get; set; }
        public string? ArchitectureOS { get; set; }
        public string? FamilyOS { get; set; }
        public string? VersionOS { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
    }

    public class UserLoginRequestDto
    {
        public string Email { get; set; }
        public string? ArchitectureOS { get; set; }
        public string? FamilyOS { get; set; }
        public string? VersionOS { get; set; }

    }

    public class CurrentUserResponseDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? GoogleId { get; set; }
        public string? Picture { get; set; }
        public DateTime Lastlogin { get; set; }
        public string? ArchitectureOS { get; set; }
        public string? FamilyOS { get; set; }
        public string? VersionOS { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }
        public string AccessToken { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class UpdateUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Picture { get; set; }
        public string? Language { get; set; }
        public string? Theme { get; set; }
    }

    public class RevokeRefreshTokenResponseDto
    {
        public string Message { get; set; }
    }

    public class RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; }
    }
}
