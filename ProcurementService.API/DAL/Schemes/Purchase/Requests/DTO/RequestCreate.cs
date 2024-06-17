using ProcurementService.API.DAL.Schemes.Purchase.Files;
using ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts;

namespace ProcurementService.API.DAL.Schemes.Purchase.Requests.DTO
{
    public class RequestCreate
    {
        public string Name { get; set; } = null!;
        public int SummaryMain { get; set; }
        public int SummarySub { get; set; }
        public RequestProductCreate[] Product { get; set; } = null!;
    }
}
