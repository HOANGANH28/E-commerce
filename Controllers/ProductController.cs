using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Controllers
{
    public class ProductController : Controller
    {
        private ecommerceContext db = new ecommerceContext();
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }
    }
}
