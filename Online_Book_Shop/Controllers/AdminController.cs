using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Online_Book_Shop.Database;
using Online_Book_Shop.Models;
using ServiceStack.Web;
using System.Net;

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
                TempData["msg"] = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage)); ;
                TempData["status"] = "1";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Book()
        {
            List<Book> books = _db.Books.ToList();
            if (!isAuthor())
            {
                ViewData["Books"] = books;
            }
            else
            {
                User u = _db.Users.Find(Convert.ToInt32(HttpContext.Session.GetString("userid")));
                if (u == null) return RedirectToAction("Login", "Home");
                List<Book> filteredBooks = new List<Book>();
                foreach (Book book in books)
                {
                    List<BookUser>authors = _db.BookUsers.Where(bu=>bu.BookId==book.BookId && bu.UserId==u.UserId).ToList();
                    if (authors != null && authors.Count>0)
                    {
                        filteredBooks.Add(book);
                    }
                }
                
                ViewData["Books"] = filteredBooks;
            }
            return View("Book");
        }
        [HttpPost]
        public IActionResult BookSubmit(IFormFile Image)   
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
            List<User> row = JsonConvert.DeserializeObject<List<User>>(Request.Form["authorData"]);
            List<User> Authors = new List<User>();
            
            using (IDbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //retriving the image
                    IFormFile img = Image;
                    if (img != null)
                    {
                        string uniqueFileName = uploadImage(img);
                        Book b = new Book(
                              Name, Width, Height, Description, pages,
                              Type.ToString(), country.ToString(), Price, ln.ToString(),
                              uniqueFileName, ISBN
                        );
                        TempData["msg"] = "New Book Successfully Added";
                        TempData["status"] = "0";
                        _db.Books.Add(b);
                        _db.SaveChanges();
                        for (int i = 0; i < row.Count; i++)
                        {
                            User temp = _db.Users.Find(row[i].UserId);
                            _db.BookUsers.Add(new BookUser(b.BookId, temp.UserId));
                        }
                        _db.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        TempData["msg"] = "Iform Image Files Is Null " + Request.Form.Files["Image"].ToString();
                        TempData["status"] = "1";
                        transaction.Rollback();
                        throw new Exception("IMage null");
                    }
                }
                catch (SqlException ex)
                {
                    TempData["msg"] = "An error occurred while saving the Book.";
                    TempData["status"] = "1";
                    transaction.Rollback();
                    throw ex;
                }
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
        public bool isAuthor()
        {
            return HttpContext.Session.GetString("userRole") == "Author";
        }
    }
}
