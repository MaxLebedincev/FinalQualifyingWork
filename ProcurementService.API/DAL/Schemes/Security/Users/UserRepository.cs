using ProcurementService.API.DAL.Core;
using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.DAL.Schemes.Security.Users
{
    public class UserRepository : BaseRepository<User>, IBaseRepository<User>
    {
        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }

        public void GetUserWithRoles(int Id)
        {

            var query = from u in _dbContext.Set<User>()
                        join ur in _dbContext.Set<UserRole>() on u.Id equals ur.UserId
                        join r in _dbContext.Set<Role>() on ur.RoleId equals r.Id
                        where u.Id == Id
                        select new
                        {
                            u.Id,
                            u.Login,
                            u.Email,
                            u.Hash,
                            u.CreatedAt,
                            u.UpdatedAt,
                            roleId = r.Id,
                            r.Name
                        };

            return;
        }
    }
}
