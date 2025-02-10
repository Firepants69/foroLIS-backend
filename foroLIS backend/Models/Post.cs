using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using foroLIS_backend.DTOs.PostDtos;

namespace foroLIS_backend.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users User { get; set; }
        
        [MaxLength(250)]
        public string ArchitectureOS { get; set; }
        [MaxLength(250)]
        public string FamilyOS {  get; set; }
        [MaxLength(50)]
        public string VersionOS { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserHistorial> UserHistorials { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<Survey> Surveries { get; set; }

        public static PostDto postToPostDto(Post post)
        {
            
            return new PostDto()
            {
                Content = post.Content,
                Title = post.Title,
                UserId = post.UserId,
                Id = post.Id,
                ArchitectureOS = post.ArchitectureOS,
                CreateAt = post.CreateAt,
                FamilyOS = post.FamilyOS,
                UpdateAt = post.UpdateAt,
                VersionOS = post.VersionOS,
            };
        }
        public static Post InsertDtoToPost(PostInsertDto dto,string userId)
        {
            return new Post()
            {
                Title = dto.Title,
                UserId = userId,
                Content = dto.Content,
                ArchitectureOS = dto.ArchitectureOS,
                FamilyOS = dto.FamilyOS,
                VersionOS = dto.VersionOS,
                CreateAt = dto.CreateAt,
            };
        }
        

    }
}
