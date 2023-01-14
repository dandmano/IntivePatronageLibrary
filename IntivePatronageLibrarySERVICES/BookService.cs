using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.QueryObjects;
using IntivePatronageLibraryCORE.Services;

namespace IntivePatronageLibrarySERVICES
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork=unitOfWork;
        }
        public async Task<PagedList<Book>> GetAllWithAuthors(BookQueryParameters bookParams)
        {
            return await _unitOfWork.Books.GetAllWithAuthorsAsync(bookParams);
        }

        public async Task<PagedList<Book>> GetAll(BookQueryParameters bookParams)
        {
            return await _unitOfWork.Books.GetAllAsync(bookParams);
        }

        public async Task<Book?> GetBookById(int id)
        {
            return await _unitOfWork.Books.GetByIdAsync(id);
        }

        public async Task<Book?> GetWithAuthorsById(int id)
        {
            return await _unitOfWork.Books.GetWithAuthorsByIdAsync(id);
        }

        public async Task<Book> AddBook(Book newBook)
        {
            await _unitOfWork.Books.AddAsync(newBook);
            await _unitOfWork.CommitAsync();
            return newBook;
        }

        public async Task UpdateBook(Book bookToBeUpdated, Book book)
        {
            bookToBeUpdated.Title=book.Title;
            bookToBeUpdated.Description=book.Description;
            bookToBeUpdated.ISBN=book.ISBN;
            bookToBeUpdated.Rating=book.Rating;
            bookToBeUpdated.PublicationDate=book.PublicationDate;

            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _unitOfWork.Books.Remove(book);
            await _unitOfWork.CommitAsync();
        }
    }
}
