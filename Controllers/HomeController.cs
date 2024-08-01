using EcommerceProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using EcommerceProject.Helper;

namespace EcommerceProject.Controllers
{
    public class HomeController : Controller
    {
        private ecommerceContext db = new ecommerceContext();
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _confEmail;

        public HomeController(ILogger<HomeController> logger, IConfiguration confEmail)
        {
            _logger = logger;
            _confEmail = confEmail;
        }

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            var categories = db.Categories.ToList();

            var viewModel = new ViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Search(string keyword)
        {
            var items = db.Products.Where(c => c.Title.Contains(keyword)).ToList();
            if (items != null && items.Any())
            {
                return PartialView("_SearchList", items);
            }
            else
            {
                return null;
            }
        }

        public ActionResult Detail(int id)
        {
            // Lấy thông tin sản phẩm từ id và truyền nó vào view chi tiết sản phẩm
            var product = db.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return RedirectToAction("Index"); 
            }
            // Lấy danh sách các bình luận của sản phẩm
            var comments = db.Comments.Where(c => c.ProductId == id).ToList();

            // Truyền sản phẩm và danh sách bình luận đến view
            var viewModel = new CommentList
            {
                Product = product,
                Comments = comments
            };

            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == productId);
            var userId = HttpContext.Session.GetInt32("Id");

            if (product != null && userId.HasValue)
            {
                // Check if the product already exists in the cart
                var cartItem = db.Carts.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId.Value);

                if (cartItem != null)
                {
                    // If the product is already in the cart, increase the quantity
                    cartItem.Quantity += quantity;
                }
                else
                {
                    // If the product is not in the cart, add it to the cart
                    cartItem = new Cart
                    {
                        ProductId = productId,
                        UserId = userId.Value,
                        Quantity = quantity,
                        Price = product.Price
                    };

                    db.Carts.Add(cartItem);
                }
                product.TotalQuantity -= quantity; // Decrease the total quantity of the product
                db.SaveChanges();

                return RedirectToAction("Cart");
            }
            else
            {
                return NotFound();
            }
        }

        public ActionResult Cart()
        {
            var userId = HttpContext.Session.GetInt32("Id");
            var cartItems = db.Carts.Include(c => c.Product).Where(c => c.UserId == userId).ToList();
            ViewBag.CartItemCount = cartItems.Count;
            return View(cartItems);
        }

        [HttpPost]
        public ActionResult DeleteCart(int id)
        {
            var item = db.Carts.Find(id);
            if (item != null)
            {
                //item.Product.TotalQuantity += item.Quantity; // Cập nhật lại tổng số lượng sản phẩm trong kho
                db.Carts.Remove(item);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult GetTotalQuantity(int productId)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                return Json(new { success = true, totalQuantity = product.TotalQuantity });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Comment(Comment model)
        {
            if (ModelState.IsValid)
            {
                // Tạo một đối tượng Comment từ dữ liệu nhận được từ form
                var comment = new Comment
                {
                    ProductId = model.ProductId,
                    User = model.User, 
                    Content = model.Content,
                    CreatedAt = DateTime.UtcNow.Date
                };

                db.Comments.Add(comment);
                db.SaveChanges();

                return RedirectToAction("Detail");
            }

            return View(model);
        }

        public ActionResult CheckOut()
        {

            var userId = HttpContext.Session.GetInt32("Id");
            var cartItems = db.Carts.Include(c => c.Product).Where(c => c.UserId == userId).ToList();
            //ViewBag.CartItemCount = cartItems.Count;
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCheckOut(CheckOut model)
        {
            if (ModelState.IsValid)
            {
                db.CheckOuts.Add(model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult SendEmail(SendEmailVM sendEmailVM)
        {
            var mailHelper = new MailHelper(_confEmail);
            if (mailHelper.Send(_confEmail["Gmail:Email"], sendEmailVM.Email, sendEmailVM.Subject, sendEmailVM.Content))
            {
                //_notyf.Success("Đã gửi email thành công");
                return Json(new { success = true });
                //return RedirectToAction("Index");
            }
            else
            {
                return Json(new { success = false });
                //_notyf.Error("Gửi email không thành công");
                //return RedirectToAction("Index");
            }
        }

        public ActionResult Register()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem email đã tồn tại trong cơ sở dữ liệu hay chưa
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError(string.Empty, "Email đã được sử dụng, vui lòng chọn email khác.");
                    return View(model);
                }

                // Mã hóa mật khẩu trước khi lưu vào cơ sở dữ liệu
                //model.Password = HashPassword(model.Password);

                db.Users.Add(model);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                ModelState.AddModelError(string.Empty, "Email và mật khẩu là bắt buộc.");
                return View(user);
            }

            var us = db.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (us != null)
            {
                HttpContext.Session.SetInt32("Id", us.Id);
                HttpContext.Session.SetString("Name", us.Name);
                HttpContext.Session.SetString("Email", us.Email);
                // Kiểm tra nếu người dùng là admin
                if (us.Email == "admin@gmail.com" && us.Password == "admin")
                {
                    // Chuyển hướng đến trang chủ của admin
                    return RedirectToAction("Home", "Admin");
                }
                else
                {
                    // Chuyển hướng đến trang người dùng thông thường
                    return RedirectToAction("Index");
                }
                
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không chính xác.");
                return View(user);
            }
        }


        public ActionResult Logout()
        {
            HttpContext.Session.Remove("Name");
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}