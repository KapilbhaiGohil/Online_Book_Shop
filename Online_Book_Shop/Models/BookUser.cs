using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    public class BookUser
    {
        [Key]
        public int BookUserId { get; set; }

        [ForeignKey("BookId")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }
        public BookUser() { }
        public BookUser(int bookid,int userid)
        {
            this.UserId = userid;
            this.BookId = bookid;
        }
    }
}
