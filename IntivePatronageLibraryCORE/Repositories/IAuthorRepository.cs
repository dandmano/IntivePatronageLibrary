using IntivePatronageLibraryCORE.Models;
using IntivePatronageLibraryCORE.Models.QueryObjects;

namespace IntivePatronageLibraryCORE.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<PagedList<Author>> GetAllAsync(AuthorQueryParameters authorParams);
        Task<PagedList<Author>> GetAllWithBooksAsync(AuthorQueryParameters authorParams);
        Task<Author?> GetWithBooksByIdAsync(int id);
	}
}