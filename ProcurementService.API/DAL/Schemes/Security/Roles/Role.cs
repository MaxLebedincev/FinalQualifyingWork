using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.DAL.Schemes.Security.Roles
{
    public class Role
    {
        public int Id { get; set; }
        public List<UserRole> UserRoles { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
