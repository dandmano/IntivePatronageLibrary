using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntivePatronageLibraryDATA.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Book>> GetAllWithAuthorsAsync()
        {
            return await LibraryDbContext.Books.Include(b => b.Authors).ToListAsync();
        }

        public async Task<Book?> GetWithAuthorsByIdAsync(int id)
        {
            return await LibraryDbContext.Books.Include(b => b.Authors).SingleOrDefaultAsync(b => b.Id == id);
        }

        private LibraryDbContext LibraryDbContext => (Db as LibraryDbContext)!;
    }
}
