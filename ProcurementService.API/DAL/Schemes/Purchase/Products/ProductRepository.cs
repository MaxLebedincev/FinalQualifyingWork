using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Purchase.Products
{
    public class ProductRepository : BaseRepository<Product>, IBaseRepository<Product>
    {
        public ProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
