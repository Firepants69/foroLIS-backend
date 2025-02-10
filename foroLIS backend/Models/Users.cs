using System.ComponentModel.DataAnnotations;
using foroLIS_backend.DTOs;
using Microsoft.AspNetCore.Identity;

namespace foroLIS_backend.Models
{
    public class Users: IdentityUser
    {
        [Key]
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? RefreshToken    { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt {  get; set; }
        public string? GoogleId { get; set; }
        public string? Picture { get; set; }
        public DateTime Lastlogin { get; set; }
        [MaxLength(250)]
        public string? ArchitectureOS { get; set; }
        [MaxLength(250)]
        public string? FamilyOS { get; set; }
        [MaxLength(50)]
        public string? VersionOS { get; set; }
        public string Language { get; set; }
        public string Theme { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserHistorial> UserHistorials { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

        public static CurrentUserResponseDto UserToCurrentUserResponseDto(Users user)
        {
            return new CurrentUserResponseDto()
            {
                Id = user.Id,
                UpdateAt = user.UpdateAt,
                Language = user.Language,
                FamilyOS = user.FamilyOS,
                ArchitectureOS  = user.ArchitectureOS,
                VersionOS   = user.VersionOS,
                CreateAt = user.CreateAt,
                Lastlogin = user.Lastlogin,
                Theme = user.Theme,
                GoogleId = user.GoogleId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Picture = user.Picture,
            };
        }
        public static UserResponseDto UsersToUsersResponseDto(Users user)
        {
            return new UserResponseDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreateAt = user.CreateAt,
                Email = user.Email,
                ArchitectureOS = user.ArchitectureOS,
                FamilyOS = user.FamilyOS,
                VersionOS = user.VersionOS,
                UpdateAt = user.UpdateAt,
                GoogleId = user.GoogleId,
                Picture = user.Picture,
                Lastlogin = user.Lastlogin,
                Theme = user.Theme,
                Language = user.Language

            };
        }
      
    }
}
