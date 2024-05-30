using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remood_Place.Data;
using Remood_Place.Models;
using System.Security.Claims;

namespace Remood_Place.Controllers
{
    public class informationuserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;

        public ApplicationDbContext Context { get; }
        public informationuserController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            Context = context;
        }
        // GET: informationuserController
        public async Task<ActionResult> IndexAsync()
        {
            var userId =User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user=await Context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            ViewBag.User = user;
            return View(user);
        }

        // GET: informationuserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: informationuserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: informationuserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: informationuserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: informationuserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }

        // GET: informationuserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: informationuserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(IndexAsync));
            }
            catch
            {
                return View();
            }
        }
    }
}
