using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.Users.DTO;
using ProcurementService.API.DAL.Schemes.Security.Users;
using ProcurementService.API.DAL.Schemes.Security.UsersRoles;
using ProcurementService.API.Tools;
using ProcurementService.API.DAL.Schemes.Purchase.Products.DTO;
using ProcurementService.API.DAL.Schemes.Purchase.Products;
using ProcurementService.API.DAL.Schemes.Purchase.Requests;
using ProcurementService.API.DAL.Schemes.Purchase.Filters;
using System.Runtime.Intrinsics.Arm;
using Microsoft.IdentityModel.Tokens;

namespace ProcurementService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "signatory, applicant")]
    public class ProductController : ControllerBase
    {
        private readonly AppSettings _conf;
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IOptions<AppSettings> conf, IUnitOfWork unitOfWork)
        {
            _conf = conf.Value;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] ProductCreate request)
        {
            var repProduct = _unitOfWork.GetRepository<Product>();
            var repFilter = _unitOfWork.GetRepository<Filter>();
            
            var date = DateTime.Now;

            var newProduct = new Product()
            {
                Name = request.Name,
                Description = request.Description,
                Type = request.Type,
                Price = request.Price,
                CreateAt = date,
                UpdateAt = date
            };

            var currentProduct = repProduct.Create(newProduct);

            await _unitOfWork.SaveChangesAsync();

            if (request.Filter is not null)
            {
                var newFilter = new Filter()
                {
                    Id = currentProduct.Id,
                    Manufacturer = request.Filter.Manufacturer,
                    VRAM = request.Filter.VRAM,
                    RAM = request.Filter.RAM,
                    SizeDisk = request.Filter.SizeDisk,
                    TypeDisk = request.Filter.TypeDisk,
                    CountCors = request.Filter.CountCors,
                    Diagonal = request.Filter.Diagonal,
                    Product = currentProduct,
                };

                var currentFilter = repFilter.Create(newFilter);

                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    var a = 56;
                }

                currentProduct.Filter = currentFilter;
            }

            return new JsonResult(new { 
                id = currentProduct.Id,
                name = currentProduct.Name,
                description = currentProduct.Description,
                Price = currentProduct.Price,
                CreateAt = date,
                UpdateAt = date,
                Type = currentProduct.Type,
                filter = currentProduct.Filter is not null
                ? new {
                    id = currentProduct.Filter.Id,
                    manufacturer = currentProduct.Filter.Manufacturer,
                    vram = currentProduct.Filter.VRAM,
                    ram = currentProduct.Filter.RAM,
                    sizeDisk = currentProduct.Filter.SizeDisk,
                    typeDisk = currentProduct.Filter.TypeDisk,
                    countCors = currentProduct.Filter.CountCors,
                    diagonal = currentProduct.Filter.Diagonal
                }
                : null

            });
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] ProductUpdate newEntity)
        {
            var repProduct = _unitOfWork.GetRepository<Product>();
            var repFilter = _unitOfWork.GetRepository<Filter>();

            var entity = await repProduct.GetAll().Include(u => u.Filter).Where(r => r.Id == id).FirstAsync();

            if (entity is null)
                return NotFound();

            if (!string.IsNullOrEmpty(newEntity.Name))
                entity.Name = newEntity.Name;

            if (!string.IsNullOrEmpty(newEntity.Description))
                entity.Description = newEntity.Description;

            if (newEntity.Price.HasValue)
                entity.Price = (decimal)newEntity.Price;

            if (newEntity.Filter is not null)
                entity.Filter = new Filter()
                {
                    Id = entity.Id,
                    Manufacturer = newEntity.Filter.Manufacturer,
                    VRAM = newEntity.Filter.VRAM,
                    RAM = newEntity.Filter.RAM,
                    SizeDisk = newEntity.Filter.SizeDisk,
                    TypeDisk = newEntity.Filter.TypeDisk,
                    Diagonal = newEntity.Filter.Diagonal,
                    CountCors = newEntity.Filter.CountCors
                };

            repProduct.Update(entity);

            if (entity.Filter is not null)
                repFilter.Update(entity.Filter);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Get([FromBody] ProductListReqeust? request)
        {
            var repProduct = _unitOfWork.GetRepository<Product>();
            var repFilter = _unitOfWork.GetRepository<Filter>();

            var list = repProduct.GetAll();
            int count = 0;

            list = list.OrderBy(e => e.Id);

            list = list.Include(p => p.Filter);


            if (request is not null)
            {
                if (!string.IsNullOrEmpty(request.Term))
                    list = list
                        .Where(e => EF.Functions.Like(e.Name, $"%{request.Term}%") || EF.Functions.Like(e.Description, $"%{request.Term}%"));

                if (request.PriceMin.HasValue && request.PriceMax.HasValue)
                {
                    list = list.Where(e => request.PriceMin <= e.Price && e.Price <= request.PriceMax);
                }
                else if (request.PriceMin.HasValue || request.PriceMax.HasValue)
                {
                    if (request.PriceMin.HasValue)
                        list = list.Where(e => request.PriceMin <= e.Price);

                    if (request.PriceMax.HasValue)
                        list = list.Where(e => e.Price <= request.PriceMax);
                }

                if (request.Filter.RAM != null && request.Filter.RAM.HasValue)
                    list = list.Where(e => e.Filter != null && e.Filter.RAM == request.Filter.RAM);

                if (request.Filter.VRAM != null &&  request.Filter.VRAM.HasValue)
                    list = list.Where(e => e.Filter != null && e.Filter.VRAM == request.Filter.VRAM);

                if (request.Filter.SizeDisk != null && request.Filter.SizeDisk.HasValue)
                    list = list.Where(e => e.Filter != null && e.Filter.SizeDisk == request.Filter.SizeDisk);

                if (request.Filter.CountCors != null && request.Filter.CountCors.HasValue)
                    list = list.Where(e => e.Filter != null && e.Filter.CountCors == request.Filter.CountCors);
                
                if (!(request.Filter.Diagonal[0] == 10 && request.Filter.Diagonal[1] == 50))
                    list = list.Where(e => e.Filter != null && request.Filter.Diagonal[0] <= e.Filter.Diagonal && e.Filter.Diagonal <= request.Filter.Diagonal[1]);

                if (!string.IsNullOrEmpty(request.Filter.Manufacturer))
                    list = list.Where(e => e.Filter != null && EF.Functions.Like(e.Filter.Manufacturer, $"%{request.Filter.Manufacturer}%"));

                if (!string.IsNullOrEmpty(request.Filter.TypeDisk))
                    list = list.Where(e => e.Filter != null && EF.Functions.Like(e.Filter.TypeDisk, $"%{request.Filter.TypeDisk}%"));
            }
            else
            {
                request = new ProductListReqeust();
            }

            count = list.Count();
            count = (count % request.Number != 0) ? (count / request.Number) + 1 : (count / request.Number);

            list = list
                .Skip((request.Offset - 1) * request.Number)
                .Take(request.Number);

            Product[] paginatedList = paginatedList = (count != 0) ? await list.ToArrayAsync() : new Product[] { };

            var resopnse = new List<object>();

            foreach (var item in paginatedList)
                resopnse.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    Description = item.Description,
                    Price = item.Price,
                    Type = item.Type,
                    Filter = item.Filter is not null 
                    ? new
                    {
                        manufacturer = item.Filter.Manufacturer,
                        typeDisk = item.Filter.TypeDisk,
                        sizeDisk = item.Filter.SizeDisk,
                        vram = item.Filter.VRAM,
                        ram = item.Filter.RAM,
                        countCors = item.Filter.CountCors,
                        Diagonal = item.Filter.Diagonal
                    }
                    : null
                });

            return new JsonResult(
                new
                {
                    list = resopnse,
                    count = count > 0 ? count : 1,
                });
        }


        //[HttpDelete("Delete/{id:int}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var rep = _unitOfWork.GetRepository<User>();

        //    var entity = await rep.GetAll().Where(r => r.Id == id).FirstAsync();

        //    if (entity is null)
        //        return NotFound();

        //    rep.Delete(entity);

        //    await _unitOfWork.SaveChangesAsync();

        //    return Ok();
        //}
    }
}
