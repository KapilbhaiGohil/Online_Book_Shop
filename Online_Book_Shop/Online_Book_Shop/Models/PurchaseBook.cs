using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    public class PurchaseBook
    {
        [Key]
        public int PurchaseBookId { get; set; }
        [Required]
        [ForeignKey("PurchaseId")]
        public int PurchaseId { get; set; }
        public Purchase purchase { get; set; }
        [Required]
        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public Book book { get; set; }
        [Required]
        public int quentity { get; set; }
        public PurchaseBook() { }
        public PurchaseBook(int bookind,int purchaseid,int quentity)
        {
            this.quentity = quentity;
            this.PurchaseId = purchaseid;
            this.BookId = bookind;
        }
    }
}
