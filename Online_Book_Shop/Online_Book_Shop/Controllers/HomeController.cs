using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Online_Book_Shop.Database;
using Online_Book_Shop.DTO;
using Online_Book_Shop.Models;
using System.Diagnostics;
using System.Security.Cryptography.Xml;

namespace Online_Book_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookingDbContext _db;

        public HomeController(ILogger<HomeController> logger, BookingDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("userid") == null){return RedirectToAction("Register");}
            List<Book> books = _db.Books.ToList();
            foreach (Book b in books)
            {
                List<BookUser> bookUser = _db.BookUsers.Where(bu => bu.BookId == b.BookId).ToList();
                List<User> Authors = new List<User>();
                foreach (BookUser bu in bookUser)
                {
                    Authors.Add(_db.Users.Find(bu.UserId));
                }
                b.Authors = Authors;
            }
            ViewData["BookList"] = books;
            return View();
        }

        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("userid") != null) { return RedirectToAction("Index"); }
            return View();
        }
        [HttpPost]
        public IActionResult Register(User u)
        {
            if (HttpContext.Session.GetString("userid") != null) { return RedirectToAction("Index"); }
            try
            {
                _db.Users.Add(u);
                _db.SaveChanges();
                TempData["msg"] = "Registration Successfull";
                TempData["status"] = 0;
                return RedirectToAction("Login");
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException && sqlException.Number == 2601)
                {
                    TempData["msg"] = "Email already exists. Please use a different email address.";
                    TempData["status"] = "1";
                }
                else
                {
                    TempData["msg"] = "An error occurred while saving the user.";
                    TempData["status"] = "1";
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                TempData["status"] = 1;
            }
            return View();
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("userid") != null) { return RedirectToAction("Index"); }
            return View();
        }
        [HttpPost]
        public IActionResult Login(User u)
        {
            if (HttpContext.Session.GetString("userid") != null) { return RedirectToAction("Index"); }
            try
            {
                User ok = _db.Users.Where(d => d.Email == u.Email && d.Password == u.Password).FirstOrDefault();
                if (ok != null)
                {
                    TempData["msg"] = "Successfull Login";
                    TempData["status"] = 0;
                    HttpContext.Session.SetString("userid", ok.UserId.ToString());
                    HttpContext.Session.SetString("userRole", ok.Role.ToString());
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Invalid Credentials - No User Found With Credentials";
                    TempData["status"] = 1;
                }
            } catch (Exception ex)
            {
                TempData["msg"] = ex.Message;
                TempData["status"] = 1;
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpPost]
        public IActionResult AddItemToCart([FromBody] AddToCart obj)
        {
            //return Json(obj);
            User u = _db.Users.Find(Convert.ToInt32(obj.userid));
            _db.Cart.Add(new Cart(u.UserId, obj.Book.BookId, 1));
            _db.SaveChanges();
            return Json("Successfull");
        }
        [HttpPost]
        public IActionResult RemoveItemFromCart([FromBody] AddToCart obj)
        {
            User u = _db.Users.Find(Convert.ToInt32(obj.userid));
            Cart remove = _db.Cart.Where(c => c.BookId == obj.Book.BookId && c.UserId == obj.userid).FirstOrDefault();
            _db.Cart.Remove(remove);
            _db.SaveChanges();
            return Json("Successfull");
        }
        [HttpPost]
        public IActionResult changeQuentity([FromBody]AddToCart obj)
        {
            User u = _db.Users.Find(Convert.ToInt32(obj.userid));
            Cart remove = _db.Cart.Where(c => c.BookId == obj.Book.BookId && c.UserId == obj.userid).FirstOrDefault();
            remove.Quentity = obj.quentity;
            _db.Cart.Update(remove);
            _db.SaveChanges();
            return Json(obj);
        }
        [HttpPost]
        public IActionResult GetCartData([FromBody]singleValue obj)
        {
            User u = _db.Users.Find(Convert.ToInt32(obj.ival));
            List<Cart> cartData = _db.Cart.Where(c => c.UserId == u.UserId).ToList();
            List<AddToCart> objs = new List<AddToCart>();
            List<Book> books = new List<Book>();
            foreach(Cart c in cartData)
            {
                Book temp = _db.Books.Find(c.BookId);
                List<BookUser>bu = _db.BookUsers.Where(bk=>bk.BookId==temp.BookId).ToList();
                List<User> authors = new List<User>();
                foreach(BookUser bk in bu)
                {
                    authors.Add(_db.Users.Find(bk.UserId));
                }
                temp.Authors = authors;
                objs.Add(new AddToCart(temp,c.Quentity));
                books.Add(temp);
            }
            return Json(objs);
        }
        [HttpPost]
        public IActionResult cartPurchase([FromBody]singleValue s)
        {
            List<Cart> cartItems = _db.Cart.Where(c => c.UserId == s.ival).ToList();
            IDbContextTransaction tran = _db.Database.BeginTransaction();
            try
            {
                int totalPrice = calculateTotalPrice(cartItems);
                Purchase p = new Purchase(totalPrice,_db.Users.Find(s.ival));
                _db.Purchase.Add(p);
                _db.SaveChanges();
                int pid = p.PurchaseId;
                foreach (Cart item in cartItems)
                {
                    Book b = _db.Books.Find(item.BookId);
                    PurchaseBook pb = new PurchaseBook(b.BookId, pid, item.Quentity);
                    _db.PurchaseBook.Add(pb);
                }
                _db.SaveChanges();
                //clear cart
                _db.Cart.Where(c => c.UserId == s.ival).ExecuteDelete();
                _db.SaveChanges();
                tran.Commit();
                return Json(pid);
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw ex;
            }
            finally
            {
                tran.Dispose();
            }
            return Json(cartItems);
        }
        public int  calculateTotalPrice(List<Cart>items)
        {
            int res = 0;
            foreach(Cart c in items)
            {
                Book b = _db.Books.Find(c.BookId);
                res += Convert.ToInt32(b.Price * c.Quentity);
            }
            return res;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}