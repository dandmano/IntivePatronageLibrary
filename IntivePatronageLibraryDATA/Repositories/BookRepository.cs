using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;

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

        //From code-maze tutorial
        private void ApplySort(ref IQueryable<Book> books, string orderByQueryString)
        {
            if (!books.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                books = books.OrderBy(x => x.Title);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;
                var propertyFromQueryName = param.Split(" ")[0];
                var objectProperty = propertyInfos.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.OrdinalIgnoreCase));
                if (objectProperty == null)
                    continue;
                var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }
            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
            {
                books = books.OrderBy(x => x.Title);
                return;
            }
            books = books.OrderBy(orderQuery);
        }
    }
}