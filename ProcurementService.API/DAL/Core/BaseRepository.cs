using Microsoft.EntityFrameworkCore;
using ProcurementService.API.DAL.Core.Interfaces;

namespace ProcurementService.API.DAL.Core
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        public BaseRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public TEntity Create(TEntity entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public void AddRange(params TEntity[] entity)
        {
            _dbSet.AddRange(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async virtual ValueTask<TEntity?> FindAsync(params object?[]? keyValues) => await _dbSet.FindAsync(keyValues);
    }
}
