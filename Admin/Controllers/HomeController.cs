using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public ecommerceContext db = new ecommerceContext();
        public ActionResult Index()
        {
            var productCount = db.Products.Count();
            var userCount = db.Users.Count();
            var categoryCount = db.Categories.Count();

            ViewBag.ProductCount = productCount;
            ViewBag.UserCount = userCount;
            ViewBag.CategoryCount = categoryCount;

            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index", "Home");
        }
    }
}
