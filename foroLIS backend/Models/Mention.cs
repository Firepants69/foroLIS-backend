using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using foroLIS_backend.DTOs;

namespace foroLIS_backend.Models
{
    public class Mention
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid? CommentId { get; set; }
        public Guid? PostId { get; set; }
        public string MentionendUserId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post? Post { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment? Comment { get; set; }
        [ForeignKey("MentionendUserId")]
        public virtual Users Users { get; set; }

    }
}
