using System.Linq.Expressions;
using IntivePatronageLibraryCORE.Models;

namespace IntivePatronageLibraryCORE.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
		Task<IEnumerable<Book>> GetAllWithAuthorsAsync();
        Task<Book?> GetWithAuthorsByIdAsync(int id);
	}
}