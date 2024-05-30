using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remood_Place.Models
{
    public class PackagePosts
    {
        [ForeignKey("Package")]
        public int PackageId { get; set; }

        public virtual Package Package { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        public virtual Post Post { get; set; }

        [Range(1, 5)]
        [Required]
        public int orderposts { get; set; }

    }
}
