using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User user { get; set; }
        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public Book book { get; set; }
        [Required]
        public int Quentity { get; set; }
        public Cart() { }
        public Cart(int UserId,int BookId,int quentity)
        {
            this.UserId = UserId;
            this.BookId = BookId;
            this.Quentity = quentity;
        }
    }
}
