namespace foroLIS_backend.DTOs.PostDtos
{
    public class PostInsertDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string ArchitectureOS { get; set; }
        public string FamilyOS { get; set; }
        public string VersionOS { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
