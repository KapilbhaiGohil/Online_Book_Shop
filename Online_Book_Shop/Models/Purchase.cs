using System.ComponentModel.DataAnnotations;

namespace Online_Book_Shop.Models
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public decimal TotalPrice { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public List<Book> Books { get; set; }
        [Required]
        public DateTime PurchaseTime { get;set; }
        public Purchase(){}
        public Purchase(decimal totalprice, User user, List<Book> books)
        {
            TotalPrice = totalprice;
            User = user;
            Books = books;
        }
    }
}
