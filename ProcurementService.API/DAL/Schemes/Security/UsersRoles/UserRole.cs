using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.Users;

namespace ProcurementService.API.DAL.Schemes.Security.UsersRoles
{
    public class UserRole
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
