using System.ComponentModel.DataAnnotations;

namespace IntivePatronageLibraryAPI.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public bool Gender { get; set; }

        public List<Book> Books { get; } = new();
    }
}
