using ProcurementService.API.DAL.Schemes.Purchase.Filters;
using ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts;

namespace ProcurementService.API.DAL.Schemes.Purchase.Products
{
    public class Product
    {
        public int Id { get; set; }
        public Filter? Filter { get; set; }
        public List<RequestProduct> RequestsProducts { get; set; } = null!;

        public string Name { get; set; } = null!;
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public decimal Price { get; set; }
        public int Type { get; set; }
    }
}
