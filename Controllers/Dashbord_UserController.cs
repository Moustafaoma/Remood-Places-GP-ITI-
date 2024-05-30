using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Remood_Place.Data;
using Remood_Place.Models;
using System.Security.Claims;

namespace Remood_Place.Controllers
{
    
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class Dashbord_UserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public Dashbord_UserController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        // GET: DashbordController
        public ActionResult Index()
        {
            return View(context.Posts.ToList());
        }

        // GET: DashbordController/Details/5
        public ActionResult Details(int id)
        {
            return View(context.Posts.Where(b=>b.Id==id).FirstOrDefault());
        }

        // GET: DashbordController/Create
        public ActionResult Create()
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Post model = new Post();
            if (UserId != null)
            {
                model.UserId = UserId;
                //model.AppUser = (AppUser)await context.Users.FirstOrDefaultAsync(u => u.Id == UserId);
            }
            return View(model);
        }

        // POST: DashbordController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                var newPost = new Post
                {
                    Name = post.Name,
                    Description = post.Description,
                    PhoneNumber = post.PhoneNumber,
                    Location = post.Location,
                    Fb_Link = post.Fb_Link,
                    Category = post.Category,
                    Vibs = post.Vibs,
                    State = post.State,
                    UserId = post.UserId,
                    ImagePaths = new List<string>()
                };

                foreach (var imageFile in images)
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        newPost.ImagePaths.Add("/images/" + uniqueFileName);
                    }
                }

                context.Posts.Add(newPost);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: DashbordController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DashbordController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DashbordController/Delete/5
        public ActionResult Delete(int id)
        {
            return View(context.Posts.Where(b => b.Id == id).FirstOrDefault());
        }

        // POST: DashbordController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAdmin(int id)
        {
            try
            {
                Post postremove = context.Posts.Where(x => x.Id == id).FirstOrDefault();
                context.Posts.Remove(postremove);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
