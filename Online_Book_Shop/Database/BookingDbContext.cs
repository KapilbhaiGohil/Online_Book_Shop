using Microsoft.EntityFrameworkCore;

namespace Online_Book_Shop.Database
{
    public class BookingDbContext:DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }

    }
}
