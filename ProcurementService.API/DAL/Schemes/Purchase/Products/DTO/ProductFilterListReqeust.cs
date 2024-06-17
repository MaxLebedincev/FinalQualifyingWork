namespace ProcurementService.API.DAL.Schemes.Purchase.Products.DTO
{
    public class ProductFilterListReqeust
    {
        public string? Manufacturer { get; set; }
        public int? VRAM { get; set; }
        public int? RAM { get; set; }
        public int? SizeDisk { get; set; }
        public string? TypeDisk { get; set; }
        public int? CountCors { get; set; }
        public int[]? Diagonal { get; set; } = new int[] { 10, 50 };
    }
}
