using ProcurementService.API.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Schemes.Security.Users.DTO;
using ProcurementService.API.DAL.Schemes.Security.Users;
using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly AppSettings _conf;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IOptions<AppSettings> conf, IUnitOfWork unitOfWork)
        {
            _conf = conf.Value;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create([FromBody] UserCreate request)
        {
            var rep = _unitOfWork.GetRepository<User>();
            var repRole = _unitOfWork.GetRepository<Role>();
            var repUserRole = _unitOfWork.GetRepository<UserRole>();

            var date = DateTime.Now;

            var newEntity = new User()
            {
                Login = request.Login,
                Email = request.Email,
                Hash = Security.GetHash($"{date}{request.Password}{date}"),
                CreatedAt = date,
                UpdatedAt = date
            };

            var currentUser = rep.Create(newEntity);

            await _unitOfWork.SaveChangesAsync();

            if (request.Roles.Length != 0)
            {
                var roles = await repRole.GetAll().ToArrayAsync();

                var successRole = new List<Role>();

                foreach (var role in roles)
                {
                    foreach(var currentRole in request.Roles)
                    {
                        if(currentRole == role.Name)
                        {
                            successRole.Add(role);
                        }
                    } 
                }

                foreach (var role in successRole)
                {
                    var newUserRole = new UserRole()
                    {
                        UserId = currentUser.Id,
                        RoleId = role.Id,
                    };

                    repUserRole.Create(newUserRole);
                }
            }
            
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] UserEdit newEntity)
        {
            var rep = _unitOfWork.GetRepository<User>();
            var repRole = _unitOfWork.GetRepository<Role>();

            var entity = await rep.GetAll().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(r => r.Id == id).FirstAsync();

            if (entity is null)
                return NotFound();

            if(!string.IsNullOrEmpty(newEntity.Login))
                entity.Login = newEntity.Login;
            
            if (!string.IsNullOrEmpty(newEntity.Email))
                entity.Email = newEntity.Email;

            if (!string.IsNullOrEmpty(newEntity.Password))
                entity.Hash = Security.GetHash($"{entity.CreatedAt}{newEntity.Password}{entity.CreatedAt}");

            var roles = await repRole.GetAll().ToArrayAsync();

            var newRole = new List<UserRole>();

            foreach (var role in newEntity.Roles)
            {
                foreach (var currentRole in roles)
                {
                    if (role == currentRole.Name)
                    {
                        var newUserRole = new UserRole()
                        {
                            RoleId = currentRole.Id,
                            UserId = entity.Id
                        };

                        newRole.Add(newUserRole);
                    }
                }
            }

            entity.UserRoles = newRole;

            entity.UpdatedAt = DateTime.Now;

            rep.Update(entity);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Get")]
        public async Task<IActionResult> Get([FromBody] UserListRequest? request)
        {
            var repUser = _unitOfWork.GetRepository<User>();
            var repRole = _unitOfWork.GetRepository<Role>();
            var repUserRole = _unitOfWork.GetRepository<UserRole>();

            var list = repUser.GetAll();
            int count = 0;

            list = list.OrderBy(e => e.Id);

            if (request is not null)
            {
                if (!string.IsNullOrEmpty(request.Term))
                    list = list
                        .Where(e => EF.Functions.Like(e.Login, $"%{request.Term}%") || EF.Functions.Like(e.Email, $"%{request.Term}%"));

            }
            else
            {
                request = new UserListRequest();
            }
            
            count = list.Count();
            count = (count % request.Number != 0) ? (count / request.Number) + 1 : (count / request.Number);

            list = list
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .Skip((request.Offset - 1) * request.Number)
                .Take(request.Number);

            User[] paginatedList = paginatedList = (count != 0) ? await list.ToArrayAsync() : new User[] { };

            var resopnse = new List<object>();

            foreach (var item in paginatedList)
                resopnse.Add(new 
                {
                    Id = item.Id,
                    Login = item.Login,
                    Email = item.Email,
                    Created = item.CreatedAt,
                    Updated = item.UpdatedAt,
                    Roles = item?.UserRoles?.Select(u => u.Role.Name).ToArray()
                });

            return new JsonResult(
                new
                {
                    list = resopnse,
                    count = count > 0 ? count : 1,
                });
        }


        [HttpDelete("Delete/{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var rep = _unitOfWork.GetRepository<User>();

            var entity = await rep.GetAll().Where(r => r.Id == id).FirstAsync();

            if (entity is null)
                return NotFound();

            rep.Delete(entity);

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("GetPerson")]
        public async Task<ActionResult> GetPerson([FromBody] UserGet request)
        {
            var rep = _unitOfWork.GetRepository<User>();

            var entity = await rep.GetAll().Where(r => r.Login == request.Login).Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstAsync();

            return new JsonResult(new
            {
                Id = entity.Id,
                Login = entity.Login,
                Email = entity.Email,
                Created = entity.CreatedAt,
                Updated = entity.UpdatedAt,
                Roles = entity?.UserRoles?.Select(u => u.Role.Name).ToArray()
            });
        }
    }
}
