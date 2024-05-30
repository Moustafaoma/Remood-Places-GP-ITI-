using Remood_Place.Models;

namespace Remood_Place.ViewModel
{
    public class HomeViewModel
    {
        public List<Post> AllPosts { get; set; }
        public List<Post> RestaurantPosts { get; set; }
        public List<Post> CafePosts { get; set; }
        public List<Post> GamePosts { get; set; }
        public List<Post> ShopPosts { get; set; }
        public List<Post> Ads { get; set; }
        public List<Post> Trend { get; set; }
        public List<Post> Offers { get; set; }

        public List<Post> UserFavoritePosts { get; set; }
        public List<Post> UserLikePosts { get; set; }

        public Category category { get; set; }
        public Vibs vibs { get; set; }



    }
}
