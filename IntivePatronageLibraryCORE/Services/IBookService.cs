using IntivePatronageLibraryCORE.Models;

namespace IntivePatronageLibraryCORE.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllWithBooks();
        Task<Author> GetAuthorById(int id);
        Task<Author> AddAuthor(Author newAuthor);
        Task DeleteAuthor(Author author);
    }
}
