using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Services;

namespace IntivePatronageLibrarySERVICES
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork=unitOfWork;
        }
        public async Task<IEnumerable<Author>> GetAllWithBooks()
        {
            return await _unitOfWork.Authors.GetAllWithBooksAsync();
        }

        public async Task<IEnumerable<Author>> GetAll()
        {
            return await _unitOfWork.Authors.GetAllAsync();
        }

        public async Task<Author?> GetAuthorById(int id)
        {
            return await _unitOfWork.Authors.GetByIdAsync(id);
        }

        public async Task<Author?> GetWithBooksById(int id)
        {
            return await _unitOfWork.Authors.GetWithBooksByIdAsync(id);
        }

        public async Task<Author> AddAuthor(Author newAuthor)
        {
            await _unitOfWork.Authors.AddAsync(newAuthor);
            await _unitOfWork.CommitAsync();
            return newAuthor;
        }

        public async Task<Author> AddBookToAuthor(int authorId, int bookId)
        {
            var author = await _unitOfWork.Authors.GetWithBooksByIdAsync(authorId);
            var book = await _unitOfWork.Books.GetByIdAsync(bookId);
            if (book != null) author?.Books.Add(book);
            await _unitOfWork.CommitAsync();
            return author;
        }

        public async Task DeleteAuthor(Author author)
        {
            _unitOfWork.Authors.Remove(author);
            await _unitOfWork.CommitAsync();
        }
    }
}
