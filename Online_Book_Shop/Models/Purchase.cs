using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Online_Book_Shop.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }
        [Required] 
        public decimal TotalPrice { get; set; }
        [Required]
        public User User { get; set; }
        public List<Book> Books { get; set; }
        [Required]
        public DateTime PurchaseTime { get;set; }= DateTime.Now;
        public Purchase(){}
        public Purchase(decimal totalprice, User user, List<Book> books)
        {
            TotalPrice = totalprice;
            User = user;
            Books = books;
        }
        public Purchase(decimal totalPrice, User user)
        {
            this.TotalPrice = totalPrice;
            this.User = user;
        }
    }
}
