using System.ComponentModel.DataAnnotations;

namespace IntivePatronageLibraryCORE.Models.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        [Required] [MaxLength(100)] public string Title { get; set; }
        [Required] public string Description { get; set; }
        [Required] public decimal Rating { get; set; }
        [Required] [MaxLength(13)] public string ISBN { get; set; }
        [Required] public DateTime PublicationDate { get; set; }

        public virtual List<AuthorDTO> Authors { get; set; } = new();
    }
}