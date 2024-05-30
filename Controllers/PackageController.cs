using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Remood_Place.Data;
using Remood_Place.Models;
using System.Security.Claims;

namespace Remood_Place.Controllers
{
    public class PackageController : Controller
    {
        private readonly ApplicationDbContext context;

        public PackageController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: PackageController
        public async Task<IActionResult> Index()
        {
            List<int> idpackages = context.PackageFavourits.Select(p => p.PackageId).ToList();
            List<string> idusers = context.PackageFavourits.Select(p => p.UserId).ToList();
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var packagesfavuser = context.PackageFavourits.Where(c=>c.UserId==userId).Select(p => p.PackageId).ToList();
            ViewBag.idpackages = idpackages;
            ViewBag.idusers = idusers;
            ViewBag.userId = userId;
            ViewBag.packagesfavuser = packagesfavuser;
            var packages = await context.Package.Where(p=>p.IsApproved==true)
                .Include(p=>p.packagePosts).ThenInclude(p=>p.Post).OrderByDescending(p=>p.ID)
                .ToListAsync();
            return View(packages);
        }

        // GET: PackageController/Details/5
        public async Task<ActionResult> DetailsAsync(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pack_details = context.PackagePosts
                .Where(p => p.PackageId == id).OrderBy(p => p.orderposts).ToList();
            List<Post> posts_package = new List<Post>();
            foreach (var post in pack_details)
            {
                Post selected = context.Posts.Where(p => p.Id == post.PostId).FirstOrDefault();
                posts_package.Add(selected);
            }
            var Comments = await context.PackageComment
                .Where(P => P.PackageId == id).ToListAsync();
            var Favorites = await context.PackageFavourits
                .Where(P => P.PackageId == id).ToListAsync();
            ViewBag.posts_package = posts_package;
            ViewBag.comments = Comments;
            ViewBag.Favorites = Favorites;
            ViewBag.user = userId;
            return View(context.Package.Where(p => p.ID == id).FirstOrDefault());
        }

        // GET: PackageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PackageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Package collection)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            collection.UserId = userId;
            if (ModelState.IsValid)
            {
                Package newpac = new Package
                {
                    IsApproved = false,
                    UserId = collection.UserId,
                    Name = collection.Name,
                    Description= collection.Description,
                    Category = collection.Category,

                };
                context.Package.Add(newpac);
                await context.SaveChangesAsync();
                var posts = context.Posts.ToList();

                return RedirectToAction("AddPost", new { id = newpac.ID ,filter = posts });
            }
            return View();
        }
        public ActionResult AddPost(int id, List<Post> filter,string? scucces)
        {
            List<Post> show;
            if (filter.Count == 0)
            {
                show= context.Posts.ToList();
            }
            else
            {
                show = filter;

            }
            
                var package = context.Package.FirstOrDefault(p => p.ID == id);
                ViewBag.id = id;
                ViewBag.posts = show;
                ViewBag.package = package;
                ViewBag.scusses = scucces;


            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPost(int id, int id_post)
        {
            //
            var check = context.PackagePosts.Select(p=>p.PostId).ToList();
            ViewBag.ckeck=check;
            //

            int order;
            var orders =context.PackagePosts.Where(p=>p.PackageId == id).Select(o=>o.orderposts).ToList();
            if(orders.Count == 0)
            {
                 order = 1;
            }
            else
            {
                 order = orders.Max() +1; 
                if (orders.Max()==5)
                {
                    return RedirectToAction("Index");
                }
            }
           
            PackagePosts collection = new PackagePosts()
            {
                PackageId = id,
                PostId = id_post,
                orderposts = order
            };

            if (ModelState.IsValid)
            {
                context.PackagePosts.Add(collection);
                await context.SaveChangesAsync();
                var posts =context.Posts.ToList();
                var success = "Add Place Sucsses";
                return RedirectToAction("AddPost", new { id = id, filter = posts , scucces =success});
            }
            return View();
        }
        // GET: PackageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View(context.Package.Where(p=>p.ID==id).FirstOrDefault());
        }

        // POST: PackageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Package collection)
        {
            Package old = context.Package.Where(p => p.ID == id).FirstOrDefault();
            old.Name= collection.Name;
            old.Description= collection.Description;
            old.Category= collection.Category;
            if (ModelState.IsValid)
            {
                context.SaveChanges();
                var newid = id;
                return RedirectToAction("Editposts", new {id = newid});
            }
            return View();
        }

        public ActionResult Editposts(int id)
        {

            return View(
                context.Package.Where(p => p.ID == id).Include(p=>p.packagePosts).ThenInclude(p=>p.Post)
                .FirstOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int idpac ,int idpost)
        {
            var old = context.PackagePosts.Where(p =>( p.PackageId == idpac && p.PostId==idpost)).FirstOrDefault();
            if (ModelState.IsValid)
            {
                context.PackagePosts.Remove(old);
                context.SaveChanges();
                var pac = idpac;
                return RedirectToAction("Editposts", new { id = pac });
            }
            return View();
        }


        // GET: PackageController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View(context.Package.Where(p => p.ID == id).FirstOrDefault());
        //}

        // POST: PackageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Package old = context.Package.Where(p => p.ID == id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                context.Package.Remove(old);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        //------------------------comments----------------------------------

        public async Task<IActionResult> CommentsPackage(int? Packageid)
        {
            if (Packageid == null)
            {
                return NotFound();
            }

            var Comments = await context.PackageComment
                .Include(p => p.Package).Include(p => p.AppUser)
                .Where(P => P.Id == Packageid).ToListAsync();

            if (Comments.Count == 0)
            {
                return NotFound();
            }

            return View(Comments);
        }
        public ActionResult CreateCommentsPackage(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCommentsPackage(int Packageid, string comment)
        {
            var newid = Packageid;
            if (comment == null)
            {
                return RedirectToAction("Details", new { id= newid });

            }
            PackageComment newcomment = new PackageComment()
            {
                Commentt = comment,
                PackageId = Packageid,
                UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            if (ModelState.IsValid)
            {
                context.PackageComment.Add(newcomment);
                await context.SaveChangesAsync();
                return RedirectToAction("Details", new { id= newid });
            }
            return View();
        }

        public async Task<IActionResult> EditComment(int id)
        {
            var comment = await context.PackageComment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid(); 
            }
            ViewBag.id = id;
            return View(comment);
        }

        // POST: YourController/EditComment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditComment(int id, string newComment)
        {
            var comment = await context.PackageComment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid(); 
            }

            comment.Commentt = newComment;
            await context.SaveChangesAsync();

            return RedirectToAction("Details", "Package", new { id = comment.PackageId });
        }

        // GET: YourController/DeleteComment/5
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await context.PackageComment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid(); 
            }

            return View(comment);
        }

        [HttpPost, ActionName("DeleteComment")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await context.PackageComment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (comment.UserId != userId)
            {
                return Forbid();
            }

            context.PackageComment.Remove(comment);
            await context.SaveChangesAsync();

            return RedirectToAction("Details", "Package", new { id = comment.PackageId });
        }


        //------------------------Favourite----------------------------------
        public async Task<IActionResult> Favouritepackage(int packageid)
        {
            if (packageid == null)
            {
                return NotFound();
            }

            var Favorites = await context.PackageFavourits
                .Where(P => P.PackageId == packageid).ToListAsync();

            if (Favorites.Count == 0)
            {
                return NotFound();
            }

            return View(Favorites);
        }
        public ActionResult CreateFavouritepac(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFavouritepackage(int packageid)
        {
            var loginuser = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<int> idpackages =context.PackageFavourits.Select(p => p.PackageId).ToList();

            if(idpackages.Contains(packageid))
            {
                List<string> idusers = context.PackageFavourits.Where(c=>c.PackageId==packageid).Select(p => p.UserId).ToList();
                if (idusers.Contains(loginuser))
                {
                    PackageFavourit removeFavorites = context.PackageFavourits.Where(p => p.PackageId == packageid).FirstOrDefault();
                    if (ModelState.IsValid)
                    {
                        context.PackageFavourits.Remove(removeFavorites);
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        
                        return RedirectToAction(nameof(Error));
                    }
                    
                }
                else
                {
                        PackageFavourit newFavorites = new PackageFavourit()
                        {
                            PackageId = packageid,
                            UserId = loginuser,
                        };
                        if (ModelState.IsValid)
                        {
                            context.PackageFavourits.Add(newFavorites);
                            await context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    return RedirectToAction(nameof(Error));
                }

            }
            else
            {
                PackageFavourit newFavorites = new PackageFavourit()
                {
                    PackageId = packageid,
                    UserId = loginuser,
                };
                if (ModelState.IsValid)
                {
                    context.PackageFavourits.Add(newFavorites);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Error));
            }
            return View();

        }
        public ActionResult RemoveFavouritepac(int id)
        {
            ViewBag.id = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFavouritepackage(int packageid)
        {
            PackageFavourit removeFavorites = context.PackageFavourits.Where(p => p.PackageId == packageid).FirstOrDefault();
            if (ModelState.IsValid)
            {
                context.PackageFavourits.Remove(removeFavorites);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> FavouriteUserpackage()
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var favouritesuser = await context.PackageFavourits
                .Include(p => p.Package).ThenInclude(p=>p.packagePosts).ThenInclude(p=>p.Post)
                .Where(o => o.UserId == UserId).ToListAsync();
            return View(favouritesuser);
        }


        //-------------search on Package--------------------------
        [HttpGet]
        public ActionResult Search(string query)
        {
            var filteredContent =new List<Package>();
            if (string.IsNullOrWhiteSpace(query))
            {
                 filteredContent = context.Package.Where(p => p.IsApproved == true).Include(p => p.packagePosts).ThenInclude(v => v.Post)
                .ToList();
            }
            else
            {
            filteredContent = context.Package.Where(m => m.Name.StartsWith(query) && m.IsApproved==true).Include(p=>p.packagePosts).ThenInclude(v=>v.Post)
                .ToList();
            }
            //ViewBag.SearchQuery = query;
            ViewBag.filter=query;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var packagesfavuser = context.PackageFavourits.Where(c => c.UserId == userId).Select(p => p.PackageId).ToList();
            ViewBag.packagesfavuser = packagesfavuser;
            return View("Index",filteredContent);
        }

        //-------------search on posts--------------------------
        [HttpGet]
        public ActionResult Mypackage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var packagesfavuser = context.Package.Where(c => c.UserId == userId && c.IsApproved == true) 
                .Include(p=>p.packagePosts).ThenInclude (v => v.Post)
                .ToList();

            ViewBag.packagesfavuser = packagesfavuser;
            return View();
        }

        [HttpGet]
        public ActionResult Searchposts(string query, int id)
        {
            var filteredContent = new List<Post>();
            List<Post> show = context.Posts.ToList();
            var package = context.Package.FirstOrDefault(p => p.ID == id);
            ViewBag.package = package;

            if (string.IsNullOrWhiteSpace(query))
            {
                filteredContent = show;
            }
            else
            {
                filteredContent = show.Where(m => m.Name.ToLower().StartsWith(query)).ToList();
            }

            ViewBag.filter = query;
            ViewBag.SearchQuery = query;
            ViewBag.posts = filteredContent;
            ViewBag.id=id;
            var newid = id;
            string suc = null;
            return View("AddPost", new { id = newid, filter = filteredContent, scucces = suc });
        }
        public async Task<IActionResult> Error()
        {
            return View();
        }
    }
}
