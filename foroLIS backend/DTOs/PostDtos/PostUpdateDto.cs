namespace foroLIS_backend.DTOs.PostDtos
{
    public class PostUpdateDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
