using System.ComponentModel.DataAnnotations;

namespace Online_Book_Shop.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public User User { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [Range(1,5)]
        public int Star { get; set; }
        [Required]
        public DateTime ReviewTime { get; set; }
        [Required]
        public Book book { get; set; }
        public Review() { }
        public Review( User user, string description, string title, int star, DateTime reviewTime, Book book)
        {
            User = user;
            Description = description;
            Title = title;
            Star = star;
            ReviewTime = reviewTime;
            this.book = book;
        }
    }
}
