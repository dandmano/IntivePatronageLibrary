using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.QueryObjects;

namespace IntivePatronageLibraryCORE.Services
{
    public interface IAuthorService
    {
        Task<PagedList<Author>> GetAllWithBooks(AuthorQueryParameters authorParams);
        Task<PagedList<Author>> GetAll(AuthorQueryParameters authorParams);
        Task<Author?> GetAuthorById(int id);
        Task<Author?> GetWithBooksById(int id);
        Task<Author> AddAuthor(Author newAuthor);
        Task<Author> AddBookToAuthor(int authorId, int bookId);
        Task DeleteAuthor(Author author);
    }
}
