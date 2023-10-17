using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Book_Shop.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string ImageFileName { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public Author( string name, int age, string imageFileName, byte[] image)
        {
            Name = name;
            Age = age;
            ImageFileName = imageFileName;
            Image = image;
        }
        public Author() { }
    }
}
