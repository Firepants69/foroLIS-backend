using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace foroLIS_backend.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [MaxLength(250)]
        public string ArchitectureOS { get; set; }
        [MaxLength(250)]
        public string FamilyOS { get; set; }
        [MaxLength(50)]
        public string VersionOS { get; set; }
        public Guid PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
        public Guid? MediaFileId { get; set; }
        [ForeignKey("MediaFileId")]
        public virtual MediaFile? MediaFile { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public ICollection<Reaction> Reactions { get; set; }

    }
}
