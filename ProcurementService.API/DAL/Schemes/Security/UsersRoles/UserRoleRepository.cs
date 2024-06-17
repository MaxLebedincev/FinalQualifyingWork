using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Security.UsersRoles
{
    public class UserRoleRepository : BaseRepository<UserRole>, IBaseRepository<UserRole>
    {
        public UserRoleRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
