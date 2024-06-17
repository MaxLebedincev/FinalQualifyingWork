using ProcurementService.API.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ProcurementService.API.DAL.Core.Interfaces;
using ProcurementService.API.DAL.Schemes.Security.Users;
using ProcurementService.API.DAL.Schemes.Security.Users.DTO;
using ProcurementService.API.DAL.Schemes.Security.Roles;
using ProcurementService.API.DAL.Schemes.Security.UsersRoles;

namespace ProcurementService.API.Controllers.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppSettings _conf;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IOptions<AppSettings> conf, IUnitOfWork unitOfWork)
        {
            _conf = conf.Value;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Test")]
        public async Task<ActionResult> Register()
        {
            return new JsonResult(new
            {
                success = "Пользователь зарегестрирован!"
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegister data)
        {

            if (string.IsNullOrEmpty(data.Login) || string.IsNullOrEmpty(data.Password) || string.IsNullOrEmpty(data.Email))
                throw new Exception();

            var repUser = _unitOfWork.GetRepository<User>();

            var users = repUser.GetAll();

            users = users.Where(u => u.Login == data.Login || u.Email == data.Email);

            var listUser = await users.ToArrayAsync();

            if (listUser.Length > 0) 
                throw new Exception();

            var now = DateTime.Now;

            repUser.Create(new User()
            {
                Login = data.Login,
                Email = data.Email,
                Hash = Security.GetHash($"{now}{data.Password}{now}"),
                CreatedAt = now,
                UpdatedAt = now
            });

            await _unitOfWork.SaveChangesAsync();

            return new JsonResult(new
            {
                success = "Пользователь зарегестрирован!"
            });
        }

        [HttpPost("Token")]
        public async Task<ActionResult> Token([FromBody] UserAuthorization data)
        {
            var identity = await GetIdentity(data.Login, data.Password);

            if (identity == null)
            {
                return new JsonResult(new { errorText = "Логин или пароль не подходят." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: _conf.AuthOptions.Issuer,
                    audience: _conf.AuthOptions.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromHours(_conf.AuthOptions.Lifetime)),
                    signingCredentials: new SigningCredentials(_conf.AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            Response.Cookies.Append("jwt", encodedJwt, new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true
            });

            var repUser = _unitOfWork.GetRepository<User>();
            var repRole = _unitOfWork.GetRepository<Role>();
            var repUserRole = _unitOfWork.GetRepository<UserRole>();

            var user = await repUser.GetAll().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(u => u.Login.ToLower() == data.Login.ToLower()).FirstOrDefaultAsync();

            var person = new
            {
                id = user.Id,
                login = identity.Name,
                email = user.Email,
                role = user?.UserRoles?.Select(u => u.Role.Name).ToArray()
            };

            return new JsonResult(person);
        }

        [HttpPost("Logout")]
        public JsonResult Logout()
        {
            Response.Cookies.Append("jwt", "", new CookieOptions()
            {
                Expires = DateTime.Now.AddDays(-1),
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return new JsonResult(new { message = "Вы успешно вышли." });
        }

        private async Task<ClaimsIdentity> GetIdentity(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password)) throw new Exception();

            var repUser = _unitOfWork.GetRepository<User>();
            var repRole = _unitOfWork.GetRepository<Role>();
            var repUserRole = _unitOfWork.GetRepository<UserRole>();

            var user = await repUser.GetAll().Include(u => u.UserRoles).ThenInclude(ur => ur.Role).Where(u => u.Login.ToLower() == login.ToLower()).FirstOrDefaultAsync();

            var clientHash = Security.GetHash($"{user.CreatedAt}{password}{user.CreatedAt}");

            if (user.Hash == clientHash)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login)
                };

                foreach (var role in user?.UserRoles)
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Role.Name));

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            throw new Exception();
        }
    }
}
