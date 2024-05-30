using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remood_Place.Data;
using Remood_Place.ViewModel;

namespace Remood_Place.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            //HomeViewModel homeViewModel = new HomeViewModel();
            //homeViewModel.AllPosts = context.Posts.Include(p=>p.Images).ToList();
            //homeViewModel.RestaurantPosts = context.Posts.Where(p=>p.Category == Models.Category.Restaurant).ToList();
            //homeViewModel.CafePosts = context.Posts.Where(p => p.Category == Models.Category.Cafe).ToList();
            //homeViewModel.GamePosts = context.Posts.Where(p => p.Category == Models.Category.Game).ToList();
            //homeViewModel.ShopPosts = context.Posts.Where(p => p.Category == Models.Category.Shop).ToList();
            //homeViewModel.Ads = context.Posts.Where(p => p.State == Models.State.Ads).ToList();
            //homeViewModel.Offers = context.Posts.Where(p => p.State == Models.State.Offers).ToList();
            //homeViewModel.Trend = context.Posts.Where(p => p.State == Models.State.Trend).ToList();
            return RedirectToAction("index" , "posts");
        }
    }
}
