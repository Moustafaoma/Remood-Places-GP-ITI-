using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Remood_Place.Data;

namespace Remood_Place.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ReviewPackage()
        {
            // Fetch all pending posts
            var pendingPackage = _context.Package.Where(p => !p.IsApproved).ToList();
            return View(pendingPackage);
        }

        [HttpPost]
        public IActionResult ApprovePackage(int PackageId)
        {
            var Package = _context.Package.FirstOrDefault(p => p.ID == PackageId);
            if (Package != null)
            {
                Package.IsApproved = true;
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewPackage");
        }

        [HttpPost]
        public IActionResult RejectPackage(int PackageId)
        {
            var Package = _context.Package.FirstOrDefault(p => p.ID == PackageId);
            if (Package != null)
            {
                _context.Package.Remove(Package);
                _context.SaveChanges();
            }
            return RedirectToAction("ReviewPackage");
        }
        public IActionResult feedback()
        {
            
            var pendingPackage = _context.Contact.ToList();
            return View(pendingPackage);
        }
    }
}
