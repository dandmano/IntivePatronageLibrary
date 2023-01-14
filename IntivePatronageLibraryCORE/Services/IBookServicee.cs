using IntivePatronageLibraryCORE.Models;

namespace IntivePatronageLibraryCORE.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllWithAuthors();
        Task<IEnumerable<Book>> GetAll();
        Task<Book?> GetBookById(int id);
        Task<Book?> GetWithAuthorsById(int id);
        Task<Book> AddBook(Book newBook);
        Task UpdateBook(Book bookToBeUpdated, Book book);
        Task DeleteBook(Book book);
    }
}
