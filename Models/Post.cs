using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Remood_Place.Models
{
    public enum State : byte
    {
        Trend = 1,
        Ads = 2,
        Offers = 4,
        Normal = 8,
    }
    public enum Category : byte
    {
        Cafe = 1,
        Restaurant = 2,
        Game = 4,
        Shop = 8,
    }
    public enum Vibs : byte
    {
        Group = 1,
        Romantic = 2,
        Familial = 4,
    }
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        
        public string PhoneNumber { get; set; }
        [Url]
        public string Location { get; set; }
        [Url]
        public string Fb_Link { get; set; }
        public Category Category { get; set; }
        public Vibs Vibs { get; set; }
        public State State { get; set; }

        [Required]
        [MaxLength(80)]
        public string Bio { get; set; }
        public int? DiscountPercentage { get; set; }


        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        public virtual AppUser? AppUser { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Rate>? Rates { get; set; }
        public List<string>? ImagePaths { get; set; }
        public ICollection<Like>? Likes { get; set; }


    }
}
