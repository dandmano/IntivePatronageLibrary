using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.QueryObjects;

namespace IntivePatronageLibraryCORE.Services
{
    public interface IBookService
    {
        Task<PagedList<Book>> GetAllWithAuthors(BookQueryParameters bookParams);
        Task<PagedList<Book>> GetAll(BookQueryParameters bookParams);
        Task<Book?> GetBookById(int id);
        Task<Book?> GetWithAuthorsById(int id);
        Task<Book> AddBook(Book newBook);
        Task UpdateBook(Book bookToBeUpdated, Book book);
        Task DeleteBook(Book book);
    }
}
