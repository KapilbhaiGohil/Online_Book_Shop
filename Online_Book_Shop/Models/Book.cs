using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public DateTime Publish { get; set; }
        [Required]
        public string  Description { get; set; }
        [Required]
        public string  Dimension { get; set; }
        [Required]
        public int Pages { get; set; }
        [Column(TypeName= "varchar(50)")]
        public BookType Type { get; set; }
        public string CustomType { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public Language Language { get; set; }
        [Required]
        [MaxLength(50)]
        public string ImageFileName { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public List<Author> Authors { get; set; }
        public List<Review> Reviews { get; set; }
        public Book( string name, DateTime publish, string description, string dimension, int pages, BookType type, string customType, int weight, string country, decimal price, Language language, string imageFileName, byte[] image, List<Author> authors, List<Review> reviews)
        {
            Name = name;
            Publish = publish;
            Description = description;
            Dimension = dimension;
            Pages = pages;
            Type = type;
            CustomType = customType;
            Weight = weight;
            Country = country;
            Price = price;
            Language = language;
            ImageFileName = imageFileName;
            Image = image;
            Authors = authors;
            Reviews = reviews;
        }
        public Book() { }
    }
    public enum Language
    {
        English,
        Hindi,
        Gujarati,
        French,
        Russian,
        Urdu,
        Marathi,
        Punjabi
    }
    public enum BookType
    {
        Mystery,
        ScienceFiction,
        Fantasy,
        Romance,
        Thriller,
        HistoricalFiction,
        Adventure,
        Travel,
        History,
        Science,
        Cookbooks,
        SelfHelp,
        Biography,
        HealthAndWellness,
        Poetry,
        Drama,
        Comedy,
        Action,
        Horror,
        Novels,
        Religious,
        Spiritual,
    }
}
