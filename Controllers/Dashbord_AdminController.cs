using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remood_Place.Data;
using Remood_Place.Models;

namespace Remood_Place.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "SuperAdmin")]

    public class Dashbard_AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public ApplicationDbContext Context { get; }
        public Dashbard_AdminController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            Context = context;
        }
        // GET: Dashbard_AdminController
        public async Task<ActionResult> Index()
        {
            var usersInUserRole = await userManager.GetUsersInRoleAsync("User");

            if (usersInUserRole == null)
            {
                return View("NoAdminUsers");
            }

            return View(usersInUserRole);
        }

        // GET: Dashbard_AdminController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // GET: Dashbard_AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashbard_AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUser model)
        {
            if(model.ProfileImage == null)
            {
                model.ProfileImage = "/images/defult.jpg";
            }
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    //ProfileImage = model.ProfileImage,


                };
                string userPWD = model.PasswordHash;
                var createUser = await userManager.CreateAsync(user, userPWD);

                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Dashbard_AdminController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Dashbard_AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, AppUser admin)
        {
            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user != null)
                {
                    user.Email = admin.Email;
                    user.PhoneNumber = admin.PhoneNumber;
                    user.UserName = admin.UserName;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update user. Please try again.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }catch (Exception ex) { return View(admin); }
            return View(admin);
        }

        // GET: Dashbard_AdminController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Dashbard_AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAdmin(string id)
        {
            try
            {
                var user = await Context.Users.FirstOrDefaultAsync(u => u.Email == id);
                if (user != null)
                {
                    Context.Users.Remove(user);
                    await Context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
