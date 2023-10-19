using Microsoft.AspNetCore.Mvc;
using Online_Book_Shop.Database;
using Online_Book_Shop.Models;
using System.Diagnostics;

namespace Online_Book_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BookingDbContext _db;
     
        public HomeController(ILogger<HomeController> logger,BookingDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        public IActionResult Index()
        {
            ViewData["BookList"] = _db.Books.ToList();
            return View();
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