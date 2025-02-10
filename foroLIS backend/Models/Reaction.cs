using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace foroLIS_backend.Models
{
    public class Reaction
    {
        [Key, Column(Order = 0), MaxLength(255)]
        public string ReactionType { get; set; }
        [Key, Column(Order = 1)]
        public Guid? CommentId { get; set; }
        [Key, Column(Order = 2)]
        public Guid? PostId { get; set; }
        [Key, Column(Order = 3)]
        public string UserId { get; set; }

        [ForeignKey("PostId")]
        public virtual Post? Post { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment? Comment { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
    }
}
