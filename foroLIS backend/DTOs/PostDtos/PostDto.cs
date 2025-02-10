using foroLIS_backend.DTOs.SurveyDtos;
using foroLIS_backend.Models;

namespace foroLIS_backend.DTOs.PostDtos
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public UserPostDto user {  get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string ArchitectureOS { get; set; }
        public string FamilyOS { get; set; }
        public string VersionOS { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public SurveyDto? Survey { get; set; }
    }
}
