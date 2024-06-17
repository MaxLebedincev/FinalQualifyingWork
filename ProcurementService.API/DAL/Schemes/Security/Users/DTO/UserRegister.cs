using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.DAL.Schemes.Security.Users.DTO
{
    public class UserRegister
    {
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
