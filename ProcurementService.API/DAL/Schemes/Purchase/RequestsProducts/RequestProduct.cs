using ProcurementService.API.DAL.Schemes.Purchase.Products;
using ProcurementService.API.DAL.Schemes.Purchase.Requests;

namespace ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts
{
    public class RequestProduct
    {
        public int RequestId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public Request Request { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}
