using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Remood_Place.Models
{
    public class Rate
    {
        [Range(0, 5)]
        public int Value { get; set; }
        public int PostId { get; set; }

        public virtual Post? Post { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }

        public virtual AppUser? AppUser { get; set; }
    }
}
