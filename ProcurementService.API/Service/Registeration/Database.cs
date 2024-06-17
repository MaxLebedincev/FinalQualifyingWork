using Microsoft.EntityFrameworkCore;
using ProcurementService.API.DAL.Core;

namespace ProcurementService.API.Service.Registeration
{
    public static class Database
    {
        public static async void MigrateApplicationAsync(this IHost web)
        {
            await using var scope = web.Services.CreateAsyncScope();
            await using var context 
                =   scope.ServiceProvider.GetService<ApplicationContext>() 
                ??  throw new ArgumentNullException("Обязательный параметр", nameof(ApplicationContext));
            
            await context.Database.MigrateAsync();
        }
    }
}
