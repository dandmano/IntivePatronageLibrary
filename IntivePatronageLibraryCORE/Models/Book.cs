using System.ComponentModel.DataAnnotations;

namespace IntivePatronageLibraryCORE.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required] [MaxLength(100)] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public decimal Rating { get; set; }
        [Required] [MaxLength(13)] public string ISBN { get; set; }
        [Required] public DateTime PublicationDate { get; set; }

        public virtual List<Author> Authors { get; } = new();
    }
}