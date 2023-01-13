using System.Linq.Expressions;
using IntivePatronageLibraryCORE.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntivePatronageLibraryDATA.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Db;

        public Repository(DbContext db)
        {
            this.Db = db;
        }
        public async Task AddAsync(TEntity entity)
        {
            await Db.Set<TEntity>().AddAsync(entity);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Db.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Db.Set<TEntity>().ToListAsync();
        }

        public ValueTask<TEntity?> GetByIdAsync(int id)
        {
            return Db.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            Db.Set<TEntity>().Remove(entity);
        }

        public Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return Db.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }
    }
}