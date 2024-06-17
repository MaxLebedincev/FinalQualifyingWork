using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ProcurementService.API.DAL.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<TEntity> GetRepository<TEntity>(bool hasCustomRepository = false) where TEntity : class;
        int SaveChanges();
        Task<int> SaveChangesAsync();
        int ExecuteSqlCommand(string sql, params object[] parameters);

    }
}
