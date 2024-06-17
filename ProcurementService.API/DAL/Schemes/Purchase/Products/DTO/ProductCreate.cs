using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace ProcurementService.API.DAL.Schemes.Purchase.Products.DTO
{
    public class ProductCreate
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int Count { get; set; }
        public int Type { get; set; }

        public ProductFilterCreate? Filter { get; set; }
    }
}
