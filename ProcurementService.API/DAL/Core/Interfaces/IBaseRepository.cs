namespace ProcurementService.API.DAL.Core.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity Create(TEntity entity);
        public void AddRange(params TEntity[] entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        ValueTask<TEntity?> FindAsync(params object?[]? keyValues);
    }
}
