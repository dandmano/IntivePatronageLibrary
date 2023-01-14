using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntivePatronageLibraryDATA.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(DbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Author>> GetAllWithBooksAsync()
        {
            return await LibraryDbContext.Author.Include(a => a.Books).ToListAsync();
        }

        public async Task<Author?> GetWithBooksByIdAsync(int id)
        {
            return await LibraryDbContext.Author.Include(a => a.Books).SingleOrDefaultAsync(a => a.Id == id);
        }

        private LibraryDbContext LibraryDbContext => (Db as LibraryDbContext)!;
    }
}