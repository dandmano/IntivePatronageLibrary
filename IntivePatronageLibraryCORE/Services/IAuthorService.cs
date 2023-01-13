using IntivePatronageLibraryCORE.Models;

namespace IntivePatronageLibraryCORE.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllWithAuthors();
        Task<Book> GetBookById(int id);
        Task<Book> AddBook(Book newBook);
        Task UpdateBook(Book bookToBeUpdated, Book book);
        Task DeleteBook(Book book);
    }
}
