using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Online_Book_Shop.Database;
using Online_Book_Shop.Models;
using ServiceStack.Web;

namespace Online_Book_Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookingDbContext _db;
        private readonly IWebHostEnvironment _environment;
        public AdminController(BookingDbContext db,IWebHostEnvironment environment) {
            _db = db;
            _environment = environment;
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
                catch(DbUpdateException ex)
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
                catch(Exception ex)
                {
                 
                    TempData["msg"] = "Can Not reach to the server right now. Pls try again after some time";
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
        [HttpPost]
        public IActionResult BookSubmit()   
        {
            try
            {
                string Name = Request.Form["Name"];
                BookType Type; Enum.TryParse(Request.Form["Type"], out Type);
                Country country; Enum.TryParse(Request.Form["Country"], out country);
                Language ln; Enum.TryParse(Request.Form["Language"], out ln);
                string Description = Request.Form["Description"];
                string Height = Request.Form["Height"];
                string Width = Request.Form["Width"];
                int pages = Convert.ToInt32(Request.Form["Pages"]);
                decimal Price = Convert.ToInt32(Request.Form["Price"]);
                string ISBN = Request.Form["ISBN"];
                string imgName = Request.Form["image"];
                List<User> row = JsonConvert.DeserializeObject<List<User>>(Request.Form["authorData"]);
                List<User> Authors = new List<User>();
                for (int i = 0; i < row.Count; i++)
                {
                    Authors.Add(_db.Users.Find(row[0].Id));
                }
                //retriving the image
               IFormFile img = Request.Form.Files["Image"];
                string uniqueFileName = uploadImage(img);
                Book b = new Book(
                    Name,Width,Height,Description,pages,
                    Type,country,Price,ln,
                    uniqueFileName,Authors,ISBN
                );
                _db.Books.Add(b);
                _db.SaveChanges();
                return Json(new { Type,country,ln});
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Book");
        }
        private string uploadImage(IFormFile imagePath)
        {
            string uploadFolder = Path.Combine(_environment.WebRootPath,"Images/BookImages/");
            string uniqueFileName = Guid.NewGuid().ToString()+"_"+imagePath.FileName;
            string filePath = Path.Combine(uploadFolder,uniqueFileName);
            using(var FileStream = new FileStream(filePath, FileMode.Create))
            {
                imagePath.CopyTo(FileStream);
            }
            return uniqueFileName;
        }
        [HttpPost]
        public IActionResult SearchTags([FromBody] searchData search)
        {

            List<User> matching = _db.Users.Where(u => 
                (u.FirstName+u.LastName).ToLower().Contains(search.search) && u.Role==Role.Author
            ).ToList();
            return Json(matching);
        }
        [HttpPost]
        public IActionResult GetData()
        {
            List<string> Type = new List<string>(Enum.GetNames(typeof(BookType)));
            List<string> Languages = new List<string>(Enum.GetNames(typeof(Language)));
            List<string> Countries = new List<string>(Enum.GetNames(typeof(Country)));
            return Json(new { Type,Languages,Countries});
        }
        public class searchData
        {
            public string search { get; set; }
        }
    }
}
