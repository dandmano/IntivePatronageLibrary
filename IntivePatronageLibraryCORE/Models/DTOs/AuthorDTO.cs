using System.ComponentModel.DataAnnotations;

namespace IntivePatronageLibraryCORE.Models.DTOs
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        [Required] [MaxLength(50)] public string FirstName { get; set; }
        [Required] [MaxLength(50)] public string LastName { get; set; }

        [Required] public DateTime BirthDate { get; set; }

        //Gender True - Man, False - Woman
        [Required] public bool Gender { get; set; }

        public virtual List<BookDTO> Books { get; set; } = new();
    }
}