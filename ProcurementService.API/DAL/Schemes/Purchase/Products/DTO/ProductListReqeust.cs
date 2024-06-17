using ProcurementService.API.DAL.Schemes.Purchase.Filters;

namespace ProcurementService.API.DAL.Schemes.Purchase.Products.DTO
{
    public class ProductListReqeust
    {
        public string? Term { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public int Offset { get; set; } = 1;
        public int Number { get; set; } = 5;
        public ProductFilterListReqeust? Filter { get; set; }
    }
}
