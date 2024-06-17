using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Security.Roles
{
    public class RoleRepository : BaseRepository<Role>, IBaseRepository<Role>
    {
        public RoleRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
