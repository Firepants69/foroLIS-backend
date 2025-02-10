using System.ComponentModel.DataAnnotations;

namespace foroLIS_backend.DTOs.PostDtos
{
    public class UserPostDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string? Picture { get; set; }
        public string? ArchitectureOS { get; set; }
        public string? FamilyOS { get; set; }
        public string? VersionOS { get; set; }
    }
}
