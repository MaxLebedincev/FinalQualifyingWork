using Microsoft.EntityFrameworkCore;
using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Core;
using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.Users;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using ProcurementService.API.DAL.Schemes.Security.UsersRoles;
using ProcurementService.API.DAL.Schemes.Purchase.Files;
using ProcurementService.API.DAL.Schemes.Purchase.Filters;
using ProcurementService.API.DAL.Schemes.Purchase.Products;
using ProcurementService.API.DAL.Schemes.Purchase.Requests;
using ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts;

namespace ProcurementService.API.Service.Registeration
{
    public static class UnitOfWork
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryFactory, DAL.Core.UnitOfWork>();
            services.AddScoped<IUnitOfWork, DAL.Core.UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddRepository<TEntity, TRepository>(this IServiceCollection services)
                 where TEntity : class
                 where TRepository : class, IBaseRepository<TEntity>
        {
            services.AddScoped<IBaseRepository<TEntity>, TRepository>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddRepository<User, UserRepository>()
                .AddRepository<Role, RoleRepository>()
                .AddRepository<UserRole, UserRoleRepository>()
                .AddRepository<ServerFile, ServerFileRepository>()
                .AddRepository<Filter, FilterRepository>()
                .AddRepository<Product, ProductRepository>()
                .AddRepository<Request, RequestRepository>()
                .AddRepository<RequestProduct, RequestProductRepository>();

            return services;
        }
    }
}
