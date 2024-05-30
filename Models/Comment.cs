using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remood_Place.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Commentt { get; set; }
        [ForeignKey("Post")]
        
        public int PostId { get; set; }

        public virtual Post? Post { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        public virtual AppUser? AppUser { get; set; }
    }
}
