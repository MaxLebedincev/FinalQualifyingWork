namespace ProcurementService.API.DAL.Schemes.Security.Users.DTO
{
    public class UserCreate
    {
        public string Login { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string[] Roles { get; set; } = new string[] { };
    }
}
