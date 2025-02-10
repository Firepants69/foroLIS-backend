using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace foroLIS_backend.Models
{
    public class UserHistorial
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Users Users { get; set; }
        public Guid PostId {  get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
    }
}
