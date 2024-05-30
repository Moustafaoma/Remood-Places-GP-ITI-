using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Remood_Place.Models
{
    public class AppUser:IdentityUser
    {
        public string? FName { get; set; }
        public string? LName { get; set; }
        public string? ProfileImage { get; set; }

        [Required]
        public override string? UserName { get; set; }

        [Required(ErrorMessage = "The UserName field is required.")]
        public override string? Email { get; set; }
        //[Required(ErrorMessage = "The UserName field is required.")]
        //public override string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "The UserName field is required.")]
        public override string? PasswordHash { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Rate>? Rates { get; set; }
        public ICollection<Like>? Likes { get; set; }

        public ICollection<PackageComment>? PackageComment { get; set; }
        public ICollection<PackageFavourit>? PackageFavourit { get; set; }
        public ICollection<Package>? packages { get; set; }
        public ICollection<Post>? posts { get; set; }


    }
}
