namespace ProcurementService.API.DAL.Schemes.Purchase.Requests.DTO
{
    public class RequestListRequest
    {
        public string? Term { get; set; }
        public int Offset { get; set; } = 1;
        public int Number { get; set; } = 10;
    }
}
