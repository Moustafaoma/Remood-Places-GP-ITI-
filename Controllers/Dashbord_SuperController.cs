using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remood_Place.Data;
using Remood_Place.Models;
using static System.Formats.Asn1.AsnWriter;

namespace Remood_Place.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class Dashbord_SuperController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public ApplicationDbContext Context { get;}
        public Dashbord_SuperController(UserManager<IdentityUser> userManager , ApplicationDbContext context)
        {
            this.userManager = userManager;
            Context = context;
        }
        // GET: Dashbord_SuperController
        public async Task<ActionResult> Index()
        {
            var usersInAdminRole = await userManager.GetUsersInRoleAsync("Admin");

            if (usersInAdminRole == null)
            {
                return View("NoAdminUsers"); 
            }

            return View(usersInAdminRole);
        }

        // GET: Dashbord_SuperController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound(); 
            }

            return View(user); 
        }


        // GET: Dashbord_SuperController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashbord_SuperController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AppUser admin)
        {
                if (ModelState.IsValid)
                {
                    var user = new IdentityUser
                    {
                        UserName = admin.UserName,
                        Email = admin.Email,
                        PhoneNumber = admin.PhoneNumber,
                    };
                    string userPWD = admin.PasswordHash;
                    var createUser = await userManager.CreateAsync(user, userPWD);

                    if (createUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    return RedirectToAction(nameof(Index));
                }
            
            return View(admin);
        }

        // GET: Dashbord_SuperController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Dashbord_SuperController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id , AppUser admin)
        {

            try
            {
                var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
                if(user != null)
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
            }
            catch
            {
                return View(admin);
            }
			return View(admin);

		}

        // GET: Dashbord_SuperController/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var user = await Context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Dashbord_SuperController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAdmin(string id)
        {
            var x = id;
            try
            {
                var user =  Context.Users.Where(u=>u.Email==x).FirstOrDefault();
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
