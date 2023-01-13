using IntivePatronageLibraryCORE.Models;
using Microsoft.EntityFrameworkCore;

namespace IntivePatronageLibraryDATA
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }

    }
}
