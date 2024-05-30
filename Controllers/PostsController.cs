using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Remood_Place.Data;
using Remood_Place.Models;
using Remood_Place.ViewModel;

namespace Remood_Place.Controllers
{
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public PostsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel();

            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user's favorite posts
            var userFavoritePosts = await context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Post)
                .ToListAsync();

            // Retrieve the user's Like posts
            var userLikePosts = await context.Likes
                .Where(f => f.UserId == userId)
                .Select(f => f.Post)
                .ToListAsync();

            // Add user's favorite posts to the view model
            homeViewModel.UserFavoritePosts = userFavoritePosts;
            homeViewModel.UserLikePosts = userLikePosts;
            homeViewModel.AllPosts = await context.Posts.Include(p => p.Comments).Include(p => p.Likes).OrderByDescending(p => p.Id).ToListAsync();
            homeViewModel.Ads = homeViewModel.AllPosts.Where(p => p.State == State.Ads).ToList();
            homeViewModel.Offers = homeViewModel.AllPosts.Where(p => p.State == State.Offers).ToList();


            return View(homeViewModel);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await context.Posts
                .Include(p => p.Comments)  // Include Comments navigation property
                .FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [Authorize]
        // GET: Posts/Create
        public async Task<IActionResult> Create()
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

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Post post, List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                var newPost = new Post
                {
                    Name = post.Name,
                    Bio = post.Bio,
                    Description = post.Description,
                    PhoneNumber = post.PhoneNumber,
                    Location = post.Location,
                    Fb_Link = post.Fb_Link,
                    Category = post.Category,
                    Vibs = post.Vibs,
                    State = post.State,
                    DiscountPercentage = post.DiscountPercentage,
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

        [Authorize]
        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            post.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (post == null)
            {
                return NotFound();
            }
            //ViewData["UserId"] = new SelectList(context.Set<AppUser>(), "Id", "Id", post.UserId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post, List<IFormFile> images)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                    if (existingPost == null)
                    {
                        return NotFound();
                    }

                    // Update existing post properties
                    existingPost.Name = post.Name;
                    existingPost.Bio = post.Bio;
                    existingPost.Description = post.Description;
                    existingPost.PhoneNumber = post.PhoneNumber;
                    existingPost.Location = post.Location;
                    existingPost.Fb_Link = post.Fb_Link;
                    existingPost.Category = post.Category;
                    existingPost.Vibs = post.Vibs;
                    existingPost.State = post.State;
                    existingPost.DiscountPercentage = post.DiscountPercentage;

                    // Handle image uploads
                    if (images != null && images.Count > 0)
                    {
                        existingPost.ImagePaths = new List<string>();

                        foreach (var image in images)
                        {
                            if (image != null && image.Length > 0)
                            {
                                // Specify the directory path to save images
                                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                                // Check if the directory exists, if not, create it
                                if (!Directory.Exists(uploadsFolder))
                                {
                                    Directory.CreateDirectory(uploadsFolder);
                                }

                                // Generate a unique file name for each image
                                var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;

                                // Combine the directory path with the file name
                                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                                // Save the image to the specified file path
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }

                                // Add the image path to the list
                                existingPost.ImagePaths.Add("/images/" + uniqueFileName);
                            }
                        }

                    }

                    // Update the post in the database
                    context.Update(existingPost);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(context.Set<AppUser>(), "Id", "Id", post.UserId);
            return View(post);
        }

        //[HttpGet]
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await context.Posts
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

        // POST: Posts/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            var post = await context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Remove images from storage directory
            foreach (var imagePath in post.ImagePaths)
            {
                var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            // Remove the post from the database
            context.Posts.Remove(post);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteImage(string imagePath)
        {
            try
            {
                // Your logic to delete the image (e.g., delete from database or file system)

                // Assuming successful deletion, return a success response
                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                // Log the error
                return BadRequest("Failed to delete image: " + ex.Message);
            }
        }


        private bool PostExists(int id)
        {
            return context.Posts.Any(e => e.Id == id);
        }

        //------------------------comments----------------------------------
        [Authorize]
        public async Task<IActionResult> AddComment(int postId, string commentText)
        {
            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                // Return a JSON response indicating the need for authentication
                return Unauthorized(new { message = "Please login first." });
            }

            // Create a new comment
            Comment newComment = new Comment
            {
                PostId = postId,
                UserId = userId,
                Commentt = commentText,
            };

            // Add the comment to the database
            context.Comments.Add(newComment);
            await context.SaveChangesAsync();

            return Ok(); // Assuming successful comment addition
        }

        //public IActionResult GetComments(int postId)
        //{
        //    var comments = context.Comments.Where(c => c.PostId == postId).OrderBy(c=>c.Id).ToList();
        //    return PartialView("_Comments", comments);
        //}




        [HttpGet]
        public async Task<IActionResult> CommentsPost(int? postId)
        {
            if (postId == null)
            {
                return NotFound();
            }

            // استرجاع التعليقات لهذا البوست
            var comments = await context.Comments
                .Include(c => c.Post)
                .Include(c => c.AppUser)
                .Where(c => c.PostId == postId)
                .ToListAsync();

            return View(comments);
        }

        //    [HttpPost]
        //    public async Task<IActionResult> CreateComment(int postId, string commentText)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // قم بإنشاء التعليق باستخدام البيانات المرسلة
        //            Comment newComment = new Comment()
        //            {
        //                PostId = postId,
        //                Commentt = commentText,
        //                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) // قم بتعيين هوية المستخدم الحالي هنا
        //            };

        //            // إضافة التعليق إلى قاعدة البيانات وحفظ التغييرات
        //            context.Comments.Add(newComment);
        //            await context.SaveChangesAsync();

        //            // قم بتحديث الصفحة أو التحويل إلى صفحة أخرى حسب الحاجة
        //            // هنا، سنقوم بتحويل المستخدم مباشرة إلى صفحة البوست بعد إضافة التعليق
        //            return RedirectToAction("Details", "Post", new { id = postId });
        //        }

        //        // إذا واجهنا أي أخطاء في النموذج، يمكن تنفيذ رمز هنا
        //        // يمكنك تحديد كيفية معالجة الأخطاء، مثل إرسال رسالة خطأ إلى المستخدم

        //        return View();
        //}


        // GET: YourController/EditComment/5
        //public async Task<IActionResult> EditComment(int id)
        //    {
        //        var comment = await context.Comments.FindAsync(id);
        //        if (comment == null)
        //        {
        //            return NotFound();
        //        }

        //        // Check if the currently logged-in user is the owner of the comment
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        if (comment.UserId != userId)
        //        {
        //            return Forbid(); // User is not authorized to edit this comment
        //        }

        //        return View(comment);
        //    }

        // POST: YourController/EditComment/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, string newComment)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Check if the currently logged-in user is the owner of the comment
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid(); // User is not authorized to edit this comment
            }

            comment.Commentt = newComment;
            await context.SaveChangesAsync();

            // Redirect back to the post details page or any other appropriate page
            //return RedirectToAction("Details", "Post", new { id = comment.PostId });
            return Ok();
        }

        // GET: YourController/DeleteComment/5
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Check if the currently logged-in user is the owner of the comment
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid(); // User is not authorized to delete this comment
            }

            return View(comment);
        }

        // POST: YourController/DeleteComment/5
        [HttpPost, ActionName("DeleteComment")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            // Check if the currently logged-in user is the owner of the comment
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid(); // User is not authorized to delete this comment
            }

            context.Comments.Remove(comment);
            await context.SaveChangesAsync();

            // Redirect back to the post details page or any other appropriate page
            //return RedirectToAction("Details", "Post", new { id = comment.PostId });
            return Ok();
        }


        //------------------------Favourite----------------------------------
        public async Task<IActionResult> UsersLovePost(int Postid)
        {
            if (Postid == null)
            {
                return NotFound();
            }

            var Users = await context.Favorites
                .Include(p => p.Post).Include(p => p.AppUser)
                .Where(P => P.PostId == Postid).Select(f => f.AppUser).ToListAsync();

            if (Users.Count == 0)
            {
                return NotFound();
            }

            return View(Users);
        }


        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int postId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // Return 401 Unauthorized status
            }

            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                // Check if the item is already in favorites
                var existingFavorite = await context.Favorites.FindAsync(postId, userId);
                if (existingFavorite != null)
                {
                    // Item already exists in favorites, handle accordingly
                    return Conflict(); // Return 409 Conflict status
                }

                // Item doesn't exist in favorites, add it
                var newFavorite = new Favorite
                {
                    PostId = postId,
                    UserId = userId
                };

                context.Favorites.Add(newFavorite);
                await context.SaveChangesAsync();

                return Ok(); // Return 200 OK status
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error status
            }
        }

        public async Task<IActionResult> RemoveFromFavorites(int id)
        {
            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the favorite record to remove
            var favorite = await context.Favorites.FindAsync(id, userId);


            if (favorite == null)
            {
                // If the favorite record is not found, return 404 Not Found
                return NotFound();
            }

            // Remove the favorite record
            context.Favorites.Remove(favorite);
            await context.SaveChangesAsync();

            // Redirect back to the post details page
            return Ok();
        }

        public async Task<IActionResult> PostsOfUserLove()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ListOfPosts = await context.Favorites.Include(p => p.Post)
                .Where(o => o.UserId == UserId).Select(f => f.Post).ToListAsync();
            return View(ListOfPosts);
        }
        //-----------------------Like----------------------------------


        public async Task<IActionResult> UsersLoveePost(int Postid)
        {
            if (Postid == null)
            {
                return NotFound();
            }

            var Users = await context.Likes
                .Include(p => p.Post).Include(p => p.AppUser)
                .Where(P => P.PostId == Postid).Select(f => f.AppUser).ToListAsync();

            if (Users.Count == 0)
            {
                return NotFound();
            }

            return View(Users);
        }
        [HttpPost]
        public async Task<IActionResult> AddToLikes(int postId)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized(); // Return 401 Unauthorized status
            }

            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                // Check if the item is already in likes
                var existingLike = await context.Likes.FindAsync(postId, userId);
                if (existingLike != null)
                {
                    // Item already exists in likes, handle accordingly
                    return Conflict(); // Return 409 Conflict status
                }

                // Item doesn't exist in likes, add it
                var newLike = new Like
                {
                    PostId = postId,
                    UserId = userId
                };

                context.Likes.Add(newLike);
                await context.SaveChangesAsync();

                // Get updated like count
                int likeCount = await context.Likes.Where(l => l.PostId == postId).CountAsync();

                return Ok(likeCount); // Return 200 OK status with updated like count
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, ex.Message); // Return 500 Internal Server Error status
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromLikes(int id)
        {
            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Find the like record to remove
            var like = await context.Likes.FindAsync(id, userId);

            if (like == null)
            {
                // If the like record is not found, return 404 Not Found
                return NotFound();
            }

            // Remove the like record
            context.Likes.Remove(like);
            await context.SaveChangesAsync();

            // Get updated like count
            int likeCount = await context.Likes.Where(l => l.PostId == id).CountAsync();

            return Ok(likeCount); // Return 200 OK status with updated like count
        }


        public async Task<IActionResult> PostsOfUserLovee()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var ListOfPosts = await context.Likes.Include(p => p.Post)
                .Where(o => o.UserId == UserId).Select(f => f.Post).ToListAsync();
            return View(ListOfPosts);
        }

        //------------------------Filter-------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("")]
        public async Task<IActionResult> FilterCategoryVibs(Category? category, Vibs? vibs)
        {
            if (category == null && vibs == null)
            {
                return RedirectToAction("Index");
            }
            HomeViewModel homeViewModel = new HomeViewModel();

            // Get the current user's ID
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user's favorite posts
            var userFavoritePosts = await context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => f.Post)
                .ToListAsync();

            // Retrieve the user's Like posts
            var userLikePosts = await context.Likes
                .Where(f => f.UserId == userId)
                .Select(f => f.Post)
                .ToListAsync();

            // Add user's favorite posts to the view model
            homeViewModel.UserFavoritePosts = userFavoritePosts;
            homeViewModel.UserLikePosts = userLikePosts;
            if (category == null && vibs != null)
            {
                homeViewModel.AllPosts = await context.Posts.Where(p => p.Vibs == vibs).Include(p => p.Comments).Include(p => p.Likes).OrderByDescending(p => p.Id).ToListAsync();
            }
            else if (category != null && vibs == null)
            {
                homeViewModel.AllPosts = await context.Posts.Where(p => p.Category == category).Include(p => p.Comments).Include(p => p.Likes).OrderByDescending(p => p.Id).ToListAsync();
            }
            else
            {
                homeViewModel.AllPosts = await context.Posts.Where(p => p.Category == category && p.Vibs == vibs).Include(p => p.Comments).Include(p => p.Likes).OrderByDescending(p => p.Id).ToListAsync();
            }
            homeViewModel.Ads = context.Posts.Where(p => p.State == State.Ads).ToList();
            homeViewModel.Offers = context.Posts.Where(p => p.State == State.Offers).ToList();


            return PartialView("index", homeViewModel);
        }


        //------------------------Rate----------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RatePost(int id, int rating)
        {
            // Find the post by ID
            var post = await context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Get the user ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the user has already rated the post
            var existingRating = await context.Rates
                .FirstOrDefaultAsync(pr => pr.PostId == id && pr.UserId == userId);

            if (existingRating != null)
            {
                // Update the existing rating
                existingRating.Value = rating;
            }
            else
            {
                // Add a new rating entry
                var newRating = new Rate
                {
                    PostId = id,
                    UserId = userId,
                    Value = rating
                };
                context.Rates.Add(newRating);
            }

            await context.SaveChangesAsync();

            // Redirect back to the post details page or any other appropriate page
            return RedirectToAction("Details", "Post", new { id });
        }
        [HttpGet]
        public ActionResult Myposts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var postscreate = context.Posts.Where(c => c.UserId == userId)
                
                .ToList();

            ViewBag.postscreate = postscreate;
            return View();
        }
    }
}
