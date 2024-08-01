using Microsoft.AspNetCore.Mvc;
using EcommerceProject.Models;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        public ecommerceContext db = new ecommerceContext();
        public IActionResult Index()
        {
            var item = db.Users;
            return View(item);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Users.Find(id);
            if (item != null)
            {
                db.Users.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
