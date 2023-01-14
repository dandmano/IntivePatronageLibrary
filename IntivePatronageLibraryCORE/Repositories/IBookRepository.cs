using System.Linq.Expressions;
using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.QueryObjects;

namespace IntivePatronageLibraryCORE.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<PagedList<Book>> GetAllAsync(BookQueryParameters bookParams);
        Task<PagedList<Book>> GetAllWithAuthorsAsync(BookQueryParameters bookParams);
        Task<Book?> GetWithAuthorsByIdAsync(int id);
	}
}