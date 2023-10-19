﻿using System.ComponentModel.DataAnnotations;
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
        public DateTime Publish { get; set; }=DateTime.Now;
        [Required]
        public string  Description { get; set; }
        [Required]
        public string  Width { get; set; }
        [Required]
        public string Height { get; set; }
        [Required]
        public int Pages { get; set; }
        [Required]
        [Column(TypeName= "varchar(50)")]
        public BookType Type { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public Country Country { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        [StringLength(50)]
        [Column(TypeName = "varchar(50)")]
        public Language Language { get; set; }
        [Required]
        public string ImageFileName { get; set; }
        public List<User> Authors { get; set; }
        [Required]
        public string ISBN { get; set; }
        public Book(string name,string width,string height, string description, int pages, BookType type, Country country, decimal price, Language language, string imageFileName, List<User> authors, string isbn)
        {
            Name = name;
            Description = description;
            Pages = pages;
            Type = type;
            Country = country;
            Price = price;
            Language = language;
            ImageFileName = imageFileName;
            Authors = authors;
            this.Width = width;
            this.Height = height;
            this.ISBN = isbn;
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
        Novel,
        Religious,
        Spiritual,
    }
    public enum Country
    {
        India,
        Japan,
        Pakistan,
        Bangladesh,
        China,
        NewZelend,
        UnitedStateOfAmerica,
        ShriLanka,

    }
}
