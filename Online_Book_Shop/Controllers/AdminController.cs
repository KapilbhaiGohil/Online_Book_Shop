using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Online_Book_Shop.Database;
using Online_Book_Shop.Models;

namespace Online_Book_Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookingDbContext _db;
        public AdminController(BookingDbContext db) {
            _db = db;
        }

        public IActionResult Index()
        {
            List<User> users = _db.Users.ToList();
            ViewData["Users"] = users;
            ViewData["msg"] = TempData["msg"];
            ViewData["status"] = TempData["status"];
            return View("User");
        }
        [HttpPost]
        public IActionResult Index(User u)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Users.Add(u);
                    _db.SaveChanges();
                    TempData["msg"] = "New User Successfully Added";
                    TempData["status"] = "0";
                }
                catch(SqlException ex)
                {
                    if(ex.Number == 2601)
                    TempData["msg"] = "Email already exists. Please use a different email address.";
                    TempData["status"] = "1";
                }catch(Exception ex)
                {
                    TempData["msg"] = "Can't reach to the server right now. Pls try again after some time";
                    TempData["status"] = "1";
                }
            }
            else
            {
                TempData["msg"] = "Invalid Details";
                TempData["status"] = "1";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Book()
        {
            List<Book> books = _db.Books.ToList();
            ViewData["Books"] = books;
            return View("Book");
        }
    }
}
