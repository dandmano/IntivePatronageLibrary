using System.Linq.Expressions;
using IntivePatronageLibraryCORE.Models;

namespace IntivePatronageLibraryCORE.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
		Task<IEnumerable<Author>> GetAllWithBooksAsync();
        Task<Author?> GetWithBooksByIdAsync(int id);
	}
}