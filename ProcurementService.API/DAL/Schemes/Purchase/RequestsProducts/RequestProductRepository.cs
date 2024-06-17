using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts
{
    public class RequestProductRepository : BaseRepository<RequestProduct>, IBaseRepository<RequestProduct>
    {
        public RequestProductRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
