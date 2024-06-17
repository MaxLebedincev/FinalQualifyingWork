using ProcurementService.API.DAL.Schemes.Purchase.Requests;

namespace ProcurementService.API.DAL.Schemes.Purchase.Files
{
    public class ServerFile
    {
        public int Id { get; set; }
        public Request Request { get; set; } = new();

        public required string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public int IdCreate { get; set; }
        public int IdUpdate { get; set; }
        public long Size { get; set; }

    }
}
