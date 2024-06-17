namespace ProcurementService.API.DAL.Schemes.Security.Users.DTO
{
    public class UserEdit
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string[] Roles { get; set; } = new string[] { }; 

    }
}
