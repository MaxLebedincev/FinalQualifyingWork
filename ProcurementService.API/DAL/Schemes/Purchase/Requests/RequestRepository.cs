using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Purchase.Requests
{
    public class RequestRepository : BaseRepository<Request>, IBaseRepository<Request>
    {
        public RequestRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
