using PagedList;
using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace EcommerceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _web;
        public ecommerceContext db = new ecommerceContext();
        public IActionResult Index(int? page)
        {
            //IEnumerable<Product> item = db.Products.OrderByDescending(x => x.Id);
            //var pageSize = 10;
            //if(page == null)
            //{
            //    page = 1;
            //}
            //var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            //item = item.ToPagedList(pageIndex, pageSize);
            //ViewBag.PageSize = pageSize;
            //ViewBag.Page = page;
            // var item = db.Products;
            IEnumerable<Product> item = db.Products.Include(p => p.Category).ToList();
            return View(item);
        }

        public ActionResult Add()
        {
            ViewBag.Category = new SelectList(db.Categories.ToList(),"Id","Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Product model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Lưu ảnh vào thư mục trên máy chủ
                    var fileName = Path.GetFileName(imageFile.FileName);
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName);
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    // Lưu đường dẫn của ảnh vào model
                    model.Image = "/image/" + fileName;
                }

                //model.createDate = DateTime.Now;              
                db.Products.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Title");
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Title");
            var item = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model , IFormFile newImage)
        {
            if (ModelState.IsValid)
            {
                // Lấy sản phẩm từ cơ sở dữ liệu bằng ID
                var existingProduct = db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == model.Id);

                if (existingProduct != null)
                {
                    // Cập nhật thông tin của sản phẩm
                    existingProduct.Title = model.Title;
                    existingProduct.Description = model.Description;
                    existingProduct.Image = model.Image;
                    existingProduct.Price = model.Price;
                    existingProduct.PriceSale = model.PriceSale;
                    existingProduct.TotalQuantity = model.TotalQuantity;

                    // Kiểm tra nếu có tệp ảnh mới được tải lên
                    if (newImage != null && newImage.Length > 0)
                    {
                        // Xóa ảnh cũ
                        if (!string.IsNullOrEmpty(existingProduct.Image))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingProduct.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Lưu ảnh mới vào thư mục trên máy chủ
                        var fileName = Path.GetFileName(newImage.FileName);
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", fileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            newImage.CopyTo(stream);
                        }

                        // Cập nhật đường dẫn ảnh mới cho sản phẩm
                        existingProduct.Image = "/image/" + fileName;
                    }

                    // Lấy danh mục từ cơ sở dữ liệu dựa trên CategoryId mới của sản phẩm
                    var category = db.Categories.Find(model.CategoryId);

                    if (category != null)
                    {
                        // Gắn danh mục đã tìm được vào sản phẩm
                        existingProduct.Category = category;
                    }

                    // Lưu thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            ViewBag.Category = new SelectList(db.Categories.ToList(), "Id", "Title", model.CategoryId);
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = db.Products.Find(id);
            if (item != null)
            {
                db.Products.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Search(string keyword)
        {
            var items = db.Products.Where(c => c.Title.Contains(keyword)).ToList();
            if (items != null && items.Any())
            {
                return PartialView("_View", items);
            }
            else
            {
                return null;
            }
        }

    }
}
