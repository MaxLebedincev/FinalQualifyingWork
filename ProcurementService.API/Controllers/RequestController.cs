using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Schemes.Purchase.Products;
using ProcurementService.API.DAL.Schemes.Purchase.Requests.DTO;
using System.Security.Claims;
using ProcurementService.API.DAL.Schemes.Purchase.Requests;
using ProcurementService.API.DAL.Schemes.Purchase.RequestsProducts;
using ProcurementService.API.DAL.Schemes.Security.Users;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using ProcurementService.API.DAL.Schemes.Purchase.Files;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http.HttpResults;
using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.Users.DTO;
using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "signatory, applicant")]
    public class RequestController : ControllerBase
    {
        private readonly AppSettings _conf;
        private readonly IUnitOfWork _unitOfWork;

        public RequestController(IOptions<AppSettings> conf, IUnitOfWork unitOfWork)
        {
            _conf = conf.Value;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] RequestCreate request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception();
            }

            var repUser = _unitOfWork.GetRepository<User>();

            var user = repUser.GetAll().Where(u => u.Login == identity.Name).First();

            var repRequest = _unitOfWork.GetRepository<Request>();
            var repRequestProduct = _unitOfWork.GetRepository<RequestProduct>();

            var date = DateTime.Now;

            var newRequest = new Request()
            {
                Name = request.Name,
                CreateAt = date,
                UpdateAt = date,
                IdCreate = user.Id,
                IdUpdate = user.Id,
                IsConfirmed = false,
                IdConfirmed = user.Id,
                SummaryMain = request.SummaryMain,
                SummarySub = request.SummarySub
            };

            var currentRequest = repRequest.Create(newRequest);

            await _unitOfWork.SaveChangesAsync();

            foreach(var product in request.Product)
            {
                var requestProduct = new RequestProduct()
                {
                    RequestId = currentRequest.Id,
                    ProductId = product.Id,
                    Count = product.Count
                };

                repRequestProduct.Create(requestProduct);
            }

            await _unitOfWork.SaveChangesAsync();

            return new JsonResult(currentRequest.Id);
        }

        [HttpPost("Get/{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var repRequest = _unitOfWork.GetRepository<Request>();
            var repProduct = _unitOfWork.GetRepository<Product>();
            var repRequestProduct = _unitOfWork.GetRepository<RequestProduct>();

            var list = await repRequest.GetAll().Include(rp => rp.RequestsProducts).ThenInclude(rp => rp.Product).ThenInclude(p => p.Filter).Where(r => r.Id == id).FirstAsync();
            
            var data = new List<object>();

            foreach (var item in list.RequestsProducts)
                data.Add(new
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Description = item.Product.Description,
                    Price = item.Product.Price,
                    Type = item.Product.Type,
                    Count = item.Count,
                    Filter = item.Product.Filter is not null
                    ? new
                    {
                        manufacturer = item.Product.Filter.Manufacturer,
                        typeDisk = item.Product.Filter.TypeDisk,
                        sizeDisk = item.Product.Filter.SizeDisk,
                        vram = item.Product.Filter.VRAM,
                        ram = item.Product.Filter.RAM,
                        countCors = item.Product.Filter.CountCors,
                        Diagonal = item.Product.Filter.Diagonal
                    }
                    : null
                });

            return new JsonResult(new
            {
                request = new 
                {
                    id = list.Id,
                    isConfirmed = list.IsConfirmed,
                    name = list.Name,
                    summaryMain = list.SummaryMain,
                    summarySub = list.SummarySub
                },
                list = data
            });
        }

        [HttpPost("Update/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RequestUpdate request)
        {
            var repRequest = _unitOfWork.GetRepository<Request>();
            var repProduct = _unitOfWork.GetRepository<Product>();
            var repRequestProduct = _unitOfWork.GetRepository<RequestProduct>();

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception();
            }

            var repUser = _unitOfWork.GetRepository<User>();

            var user = repUser.GetAll().Where(u => u.Login == identity.Name).First();

            var requestCurrent = await repRequest.GetAll().Where(r => r.Id == id).FirstAsync();

            var date = DateTime.Now;

            requestCurrent.Name = request.Name;
            requestCurrent.SummaryMain = request.SummaryMain;
            requestCurrent.SummarySub = request.SummarySub;
            requestCurrent.CreateAt = date;
            requestCurrent.UpdateAt = date;
            requestCurrent.IdUpdate = user.Id;
            requestCurrent.IdConfirmed = user.Id;

            repRequest.Update(requestCurrent);

            await _unitOfWork.SaveChangesAsync();

            await repRequestProduct.GetAll().Where(r => r.RequestId == id).ExecuteDeleteAsync();

            await _unitOfWork.SaveChangesAsync();

            foreach (var product in request.Product)
            {
                var requestProduct = new RequestProduct()
                {
                    RequestId = id,
                    ProductId = product.Id,
                    Count = product.Count
                };

                repRequestProduct.Create(requestProduct);
            }

            await _unitOfWork.SaveChangesAsync();

            return new JsonResult(id);
        }

        [HttpPost("Sign/{id:int}")]
        public async Task<IActionResult> Sign(int id)
        {
            var repRequest = _unitOfWork.GetRepository<Request>();
            var repProduct = _unitOfWork.GetRepository<Product>();
            var repRequestProduct = _unitOfWork.GetRepository<RequestProduct>();
            var repServerFile = _unitOfWork.GetRepository<ServerFile>();

            var directoryTemplate = AppDomain.CurrentDomain.BaseDirectory + _conf.Directories.Template + $@"\";
            var directoryStorage = AppDomain.CurrentDomain.BaseDirectory + _conf.Directories.Storage + $@"\";
            var directoryStorageZip = AppDomain.CurrentDomain.BaseDirectory + _conf.Directories.StorageZip + $@"\";

            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                throw new Exception();
            }

            var repUser = _unitOfWork.GetRepository<User>();

            var user = repUser.GetAll().Where(u => u.Login == identity.Name).First();

            var requestCurrent = await repRequest.GetAll().Where(r => r.Id == id).FirstAsync();

            var date = DateTime.Now;

            requestCurrent.CreateAt = date;
            requestCurrent.UpdateAt = date;
            requestCurrent.IsConfirmed = true;
            requestCurrent.IdUpdate = user.Id;
            requestCurrent.IdConfirmed = user.Id;

            repRequest.Update(requestCurrent);

            await _unitOfWork.SaveChangesAsync();

            var list = await repRequest.GetAll().Include(rp => rp.RequestsProducts).ThenInclude(rp => rp.Product).ThenInclude(p => p.Filter).Where(r => r.Id == id).FirstAsync();

            var tableTypeOne = new List<object>();
            int i = 1;
            decimal sumTypeOne = 0;

            var tableTypeTwo = new List<object>();
            int g = 1;
            decimal sumTypeTwo = 0;


            foreach (var rp in list.RequestsProducts)
            {
                if (rp.Product.Type == 1)
                {
                    tableTypeOne.Add(new
                    {
                        number = i++,
                        name = rp.Product.Name,
                        countName = "шт",
                        count = rp.Count,
                        price = rp.Product.Price,
                        summary = rp.Count * rp.Product.Price
                    });

                    sumTypeOne += rp.Count * rp.Product.Price;
                }

                if (rp.Product.Type == 2)
                {
                    tableTypeTwo.Add(new
                    {
                        number = g++,
                        name = rp.Product.Name,
                        har = "",
                        countName = "шт",
                        count = rp.Count,
                        price = rp.Product.Price,
                        summary = rp.Count * rp.Product.Price
                    });

                    sumTypeTwo += rp.Count * rp.Product.Price;
                }
            }

            tableTypeOne.Add(new
            {
                number = "",
                name = "",
                countName = "",
                count = "",
                price = "",
                summary = sumTypeOne
            });

            tableTypeTwo.Add(new
            {
                number = "",
                name = "",
                har = "",
                countName = "",
                count = "",
                price = "",
                summary = sumTypeTwo
            });

            var book = new XLWorkbook(@$"{directoryTemplate}\Template_Type_1.xlsx");
            var workSheet = book.Worksheet(1);
            workSheet.Cell(6, 1).InsertData(tableTypeOne);

            if (System.IO.File.Exists($@"{directoryStorage}\{id}\Заявка оборудование Компьютерная школа ФИСТ.xlsx"))
            {
                System.IO.File.Delete($@"{directoryStorage}\{id}\Заявка оборудование Компьютерная школа ФИСТ.xlsx");
            }

            book.SaveAs($@"{directoryStorage}\{id}\Заявка оборудование Компьютерная школа ФИСТ.xlsx");

            
            
            var book2 = new XLWorkbook($@"{directoryTemplate}\Template_Type_2.xlsx");
            var workSheet2 = book2.Worksheet(1);
            workSheet2.Cell(4, 1).InsertData(tableTypeTwo);

            if (System.IO.File.Exists($@"{directoryStorage}\{id}\Заявка канцтовары Северная долина 2023-2024.xlsx"))
            {
                System.IO.File.Delete($@"{directoryStorage}\{id}\Заявка канцтовары Северная долина 2023-2024.xlsx");
            }

            book2.SaveAs($@"{directoryStorage}\{id}\Заявка канцтовары Северная долина 2023-2024.xlsx");



            Directory.CreateDirectory(@$"{directoryStorageZip}\{id}");

            if (System.IO.File.Exists(@$"{directoryStorageZip}\{id}\{requestCurrent.Name}.zip"))
            {
                System.IO.File.Delete(@$"{directoryStorageZip}\{id}\{requestCurrent.Name}.zip");
            }

            ZipFile.CreateFromDirectory(@$"{directoryStorage}\{id}", @$"{directoryStorageZip}\{id}\{requestCurrent.Name}.zip");

            var size = new System.IO.FileInfo(@$"{directoryStorageZip}\{id}\{requestCurrent.Name}.zip").Length;

            var serverFile = await repServerFile.GetAll().Where(sf => sf.Id == id).FirstOrDefaultAsync();

            if(serverFile != null)
            {
                serverFile.Name = $@"{requestCurrent.Name}.zip";
                serverFile.Request = list.RequestsProducts.First().Request;
                serverFile.CreateAt = date;
                serverFile.UpdateAt = date;
                serverFile.IdCreate = user.Id;
                serverFile.IdUpdate = user.Id;
                serverFile.Size = size;

                repServerFile.Update(serverFile);
            }
            else
            {
                serverFile = new ServerFile()
                {
                    Id = id,
                    Request = list.RequestsProducts.First().Request,
                    Name = $@"{requestCurrent.Name}.zip",
                    CreateAt = date,
                    UpdateAt = date,
                    IdCreate = user.Id,
                    IdUpdate = user.Id,
                    Size = size
                };

                repServerFile.Create(serverFile);
            }

            await _unitOfWork.SaveChangesAsync();

            return new JsonResult(id);
        }

        [HttpGet("GetFile/{id:int}")]
        public async Task<IActionResult> GetFile(int id)
        {
            var repServerFile = _unitOfWork.GetRepository<ServerFile>();

            var serverFile = repServerFile.GetAll().Where(sf => sf.Id == id).First();
            var directoryStorageZip = AppDomain.CurrentDomain.BaseDirectory + _conf.Directories.StorageZip + $@"\";
            
            byte[] bytes = System.IO.File.ReadAllBytes($@"{directoryStorageZip}\{id}\{serverFile.Name}");


            return File(bytes, "application/octet-stream", @$"{serverFile.Name}.zip");
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Get([FromBody] RequestListRequest? request)
        {
            var repRequest = _unitOfWork.GetRepository<Request>();

            var list = repRequest.GetAll();
            int count = 0;

            list = list.OrderBy(e => e.Id);

            if (request is not null)
            {
                if (!string.IsNullOrEmpty(request.Term))
                    list = list
                        .Where(e => EF.Functions.Like(e.Name, $"%{request.Term}%"));

            }
            else
            {
                request = new RequestListRequest();
            }

            count = list.Count();
            count = (count % request.Number != 0) ? (count / request.Number) + 1 : (count / request.Number);

            list = list
                .Skip((request.Offset - 1) * request.Number)
                .Take(request.Number);

            Request[] paginatedList = paginatedList = (count != 0) ? await list.ToArrayAsync() : new Request[] { };

            var resopnse = new List<object>();

            foreach (var item in paginatedList)
                resopnse.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreateAt = item.CreateAt,
                    UpdateAt = item.UpdateAt,
                    IsConfirmed = item.IsConfirmed,
                    SummaryMain = item.SummaryMain,
                    SummarySub = item.SummarySub,
                });

            return new JsonResult(
                new
                {
                    list = resopnse,
                    count = count > 0 ? count : 1,
                });
        }
    }
}
