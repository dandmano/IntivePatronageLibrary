using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Repositories;
using IntivePatronageLibraryDATA.Repositories;

namespace IntivePatronageLibraryDATA
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _db;
        private AuthorRepository _authorRepository;
        private BookRepository _bookRepository;

        public UnitOfWork(LibraryDbContext db)
        {
            this._db = db;
        }

        public IAuthorRepository Authors => _authorRepository ??= new AuthorRepository(_db);

        public IBookRepository Books => _bookRepository ??= new BookRepository(_db);

        public async Task<int> CommitAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
