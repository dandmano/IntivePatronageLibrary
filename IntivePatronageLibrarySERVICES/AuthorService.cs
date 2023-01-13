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

        public async Task<Author> AddAuthor(Author newAuthor)
        {
            await _unitOfWork.Authors.AddAsync(newAuthor);
            await _unitOfWork.CommitAsync();
            return newAuthor;
        }

        public async Task DeleteAuthor(Author author)
        {
            _unitOfWork.Authors.Remove(author);
            await _unitOfWork.CommitAsync();
        }
    }
}
