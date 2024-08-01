using EcommerceProject.Helper;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public ecommerceContext db = new ecommerceContext();
        public IActionResult Index()
        {
            var item = db.Categories;
            return View(item);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            if (ModelState.IsValid)
            {
                //model.createDate = DateTime.Now;
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var item = db.Categories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                //model.createDate = DateTime.Now;
                db.Categories.Attach(model);
                db.Entry(model).Property(x => x.Title).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Categories.Find(id);
            if (item != null)
            {
                db.Categories.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Search(string keyword)
        {
            var item = db.Categories.Where(c => c.Title.Contains(keyword)).ToList();
            return View("Index",item);
        }

    }
}
