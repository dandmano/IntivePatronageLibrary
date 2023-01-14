using IntivePatronageLibraryCORE.Models;

namespace IntivePatronageLibraryCORE.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllWithBooks();
        Task<IEnumerable<Author>> GetAll();
        Task<Author?> GetAuthorById(int id);
        Task<Author?> GetWithBooksById(int id);
        Task<Author> AddAuthor(Author newAuthor);
        Task<Author> AddBookToAuthor(int authorId, int bookId);
        Task DeleteAuthor(Author author);
    }
}
