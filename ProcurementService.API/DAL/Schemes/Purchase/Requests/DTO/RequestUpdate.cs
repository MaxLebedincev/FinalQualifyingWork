namespace ProcurementService.API.DAL.Schemes.Purchase.Requests.DTO
{
    public class RequestUpdate
    {
        public string Name { get; set; } = null!;
        public int SummaryMain { get; set; }
        public int SummarySub { get; set; }
        public RequestProductCreate[] Product { get; set; } = null!;
    }
}
