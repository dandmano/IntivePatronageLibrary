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
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(DbContext db) : base(db)
        {
        }

        public async Task<PagedList<Book>> GetAllAsync(BookQueryParameters bookParams)
        {
            var books = FindAll();

            SearchWithParams(ref books, bookParams);
            ApplySort(ref books, bookParams.OrderBy);

            return await PagedList<Book>.ToPagedListAsync(books, bookParams.PageNumber, bookParams.PageSize);
        }

        public async Task<PagedList<Book>> GetAllWithAuthorsAsync(BookQueryParameters bookParams)
        {
            var books = FindAll().Include(x=>x.Authors) as IQueryable<Book>;

            SearchWithParams(ref books, bookParams);
            ApplySort(ref books, bookParams.OrderBy);

            return await PagedList<Book>.ToPagedListAsync(books, bookParams.PageNumber, bookParams.PageSize);
        }

        public async Task<Book?> GetWithAuthorsByIdAsync(int id)
        {
            return await LibraryDbContext.Books.Include(b => b.Authors).SingleOrDefaultAsync(b => b.Id == id);
        }

        private LibraryDbContext LibraryDbContext => (Db as LibraryDbContext)!;

        private void SearchWithParams(ref IQueryable<Book> books, BookQueryParameters bookParams)
        {
            if (bookParams.Title != null)
                books = books.Where(x => x.Title.ToLower() == bookParams.Title.ToLower());
            if (bookParams.Description != null)
                books = books.Where(x => x.Description.ToLower() == bookParams.Description.ToLower());
            if (bookParams.Rating != null)
                books = books.Where(x => x.Rating == bookParams.Rating);
            if (bookParams.Rating_from != null && bookParams.Rating_to != null)
                books = books.Where(x => x.Rating <= bookParams.Rating_to && x.Rating >= bookParams.Rating_from);
            else if (bookParams.Rating_from != null)
                books = books.Where(x => x.Rating >= bookParams.Rating_from);
            else if (bookParams.Rating_to != null)
                books = books.Where(x => x.Rating <= bookParams.Rating_to);
            if (bookParams.ISBN != null)
                books = books.Where(x => x.ISBN.ToLower() == bookParams.ISBN.ToLower());
            if (bookParams.PublicationDate != null)
                books = books.Where(x => x.PublicationDate.Equals(bookParams.PublicationDate));
        }

        //From code-maze tutorial
        private void ApplySort(ref IQueryable<Book> books, string orderByQueryString)
        {
            if (!books.Any())
                return;
            if (string.IsNullOrWhiteSpace(orderByQueryString))
            {
                books = books.OrderBy(x => x.Id);
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
                books = books.OrderBy(x => x.Id);
                return;
            }
            books = books.OrderBy(orderQuery);
        }
    }
}