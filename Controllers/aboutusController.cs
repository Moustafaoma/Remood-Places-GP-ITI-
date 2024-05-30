using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Remood_Place.Controllers
{
    public class aboutusController : Controller
    {
        // GET: aboutusController
        public ActionResult Index()
        {
            return View();
        }

        // GET: aboutusController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: aboutusController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: aboutusController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        // GET: aboutusController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: aboutusController/Edit/5
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

        // GET: aboutusController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: aboutusController/Delete/5
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
