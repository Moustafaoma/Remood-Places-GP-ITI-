using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remood_Place.Models
{
    public enum CategoryPackage : byte
    {
        Only=1,
        Group = 2,
        Romantic = 4,
        Familial = 8,

    }
    public class Package
    {

        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        public CategoryPackage Category { get; set; }
        [ForeignKey("AppUser")]
        public string? UserId { get; set; }

        public virtual AppUser? AppUser { get; set; }

        public ICollection<PackageComment>? packageComments { get; set; }
        public ICollection<PackageFavourit>? packageFavourits { get; set; }
        public ICollection<PackagePosts>? packagePosts { get; set; }
    }
}
