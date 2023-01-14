using System.Linq.Expressions;

namespace IntivePatronageLibraryCORE.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity?> GetByIdAsync(int id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> FindAll();
        Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        void Remove(TEntity entity);
    }
}