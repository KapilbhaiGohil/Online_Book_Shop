using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using Online_Book_Shop.Models;

namespace Online_Book_Shop.DTO
{
    public class Dto
    {

    }
    public class AddToCart
    {
        public int userid { get; set; }
        public Book Book { get; set; }
        public AddToCart(int userid,Book book) {
            this.userid = userid;
            this.Book = book;
        }
        public AddToCart(Book book,int quentity)
        {
            this.Book = book;
            this.quentity = quentity;
        }
        public AddToCart() { }
        public int quentity { get; set; }
    }
    public class singleValue
    {
        public int ival { get; set; }
        public string sval { get; set; }

    }
}
