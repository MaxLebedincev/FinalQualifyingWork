namespace ProcurementService.API.DAL.Schemes.Purchase.Products.DTO
{
    public class ProductUpdate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Count { get; set; }
        public ProductFilterCreate? Filter { get; set; }
    }
}
