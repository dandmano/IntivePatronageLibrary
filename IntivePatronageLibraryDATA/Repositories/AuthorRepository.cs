using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
using IntivePatronageLibraryCORE;
using IntivePatronageLibraryCORE.Models.QueryObjects;

namespace IntivePatronageLibraryDATA.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(DbContext db) : base(db)
        {
        }

        public async Task<PagedList<Author>> GetAllAsync(AuthorQueryParameters authorParams)
        {
            var authors = FindAll();

            SearchWithParams(ref authors, authorParams);
            ApplySort(ref authors, authorParams.OrderBy);

            return await PagedList<Author>.ToPagedListAsync(authors, authorParams.PageNumber, authorParams.PageSize);
        }

        public async Task<PagedList<Author>> GetAllWithBooksAsync(AuthorQueryParameters authorParams)
        {
            var authors = FindAll().Include(x=>x.Books) as IQueryable<Author>;

            SearchWithParams(ref authors, authorParams);
            ApplySort(ref authors, authorParams.OrderBy);

            return await PagedList<Author>.ToPagedListAsync(authors, authorParams.PageNumber, authorParams.PageSize);
        }

        public async Task<Author?> GetWithBooksByIdAsync(int id)
        {
            return await LibraryDbContext.Author.Include(a => a.Books).SingleOrDefaultAsync(a => a.Id == id);
        }

        private LibraryDbContext LibraryDbContext => (Db as LibraryDbContext)!;

        private void SearchWithParams(ref IQueryable<Author> authors, AuthorQueryParameters authorParams)
        {
            if (authorParams.LastName != null)
                authors = authors.Where(
                    x => x.LastName.ToLower() == authorParams.LastName.ToLower());
            if (authorParams.FirstName != null)
                authors = authors.Where(x => x.FirstName.ToLower() == authorParams.FirstName.ToLower());
            if (authorParams.BirthDate != null)
                authors = authors.Where(x => x.BirthDate.Equals(authorParams));
            if (authorParams.Gender != null)
                authors = authors.Where(x => x.Gender == authorParams.Gender);
        }


        private void ApplySort(ref IQueryable<Author> authors, string orderByQueryString)
        {
            if (!authors.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                authors = authors.OrderBy(x => x.Id);
                return;
            }
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfos = typeof(Author).GetProperties(BindingFlags.Public | BindingFlags.Instance);
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
                authors = authors.OrderBy(x => x.Id);
                return;
            }
            authors = authors.OrderBy(orderQuery);
        }
    }
}