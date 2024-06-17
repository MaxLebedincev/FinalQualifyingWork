using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.DAL.Schemes.Security.Users
{
    public class User
    {
        public int Id { get; set; }
        public List<UserRole> UserRoles { get; set; } = null!;

        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Hash { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
