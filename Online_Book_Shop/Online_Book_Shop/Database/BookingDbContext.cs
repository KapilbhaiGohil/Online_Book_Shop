using Microsoft.EntityFrameworkCore;
using Online_Book_Shop.Models;

namespace Online_Book_Shop.Database
{
    public class BookingDbContext : DbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<BookUser> BookUsers { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<PurchaseBook> PurchaseBook{get;set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
        }
    }
}
