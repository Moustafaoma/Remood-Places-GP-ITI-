using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remood_Place.Models
{
    public class PackageComment
    {
        public int Id { get; set; }
        [Required]
        public string Commentt { get; set; }
        [ForeignKey("Package")]
        public int PackageId { get; set; }
        public virtual Package Package { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        public virtual AppUser AppUser { get; set; }
    }
}
