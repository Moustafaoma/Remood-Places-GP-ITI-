using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remood_Place.Data;
using Remood_Place.Models;

namespace Remood_Place.Controllers
{
    public class contactusController : Controller
    {
        private readonly ApplicationDbContext context;

        public contactusController(ApplicationDbContext context)
        {
            this.context = context;
        }
        // GET: contactusController
        public ActionResult Index()
        {
            return View();
        }

        // GET: contactusController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

    

        // POST: contactusController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string email ,string name ,string message)
        {
            if (ModelState.IsValid)
            {
                var newcontact = new Contact
                {
                    Name = name,
                    Email = email,
                    Descripthon = message
                };
                context.Contact.Add(newcontact);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
                return View();
            
        }

        // GET: contactusController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: contactusController/Edit/5
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

        // GET: contactusController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: contactusController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
