using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Purchase.Filters
{
    public class FilterRepository : BaseRepository<Filter>, IBaseRepository<Filter>
    {
        public FilterRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
