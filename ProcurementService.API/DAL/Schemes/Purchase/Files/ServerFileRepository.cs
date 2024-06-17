using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.DAL.Schemes.Purchase.Files
{
    public class ServerFileRepository : BaseRepository<ServerFile>, IBaseRepository<ServerFile>
    {
        public ServerFileRepository(ApplicationContext dbContext) : base(dbContext)
        {
        }
    }
}
