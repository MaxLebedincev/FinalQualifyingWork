namespace ProcurementService.API.DAL.Schemes.Security.Users.DTO
{
    public class UserListRequest
    {
        public string? Term { get; set; }
        public int Offset { get; set; } = 1;
        public int Number { get; set; } = 10;
    }
}
