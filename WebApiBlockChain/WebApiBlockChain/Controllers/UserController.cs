using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;
using WebApiBlockChain.Service;
using Microsoft.AspNetCore.Authorization;

namespace WebApiBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly EntityGateway _db;
        private readonly IBlockService _service;
        public UserController(EntityGateway dbContext, IBlockService service)
        {
            _db = dbContext;
            _service = service;
        }

        // Получение списка всех пользователей
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _db.GetUsers();
            return Ok(users);
        }

        // Создание нового пользователя
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data");
            }
            if (user.Role == "Admin")
            {
                user.Role = "Customer";
            }
            user.Password = _service.GetHash(user.Password);
            _db.AddUser(user);

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        //Авторизация пользователя
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User request)
        {
            // Проверяем, что тело запроса не пустое
            if (request == null)
            {
                return BadRequest("Отсутствуют данные для авторизации");
            }

            // Получаем пользователя из базы данных по указанному логину
            var user = _db.GetUsers(u => u.Name == request.Name).FirstOrDefault();

            // Если пользователь не найден, возвращаем ошибку
            if (user == null)
            {
                return Unauthorized("Неверный логин или пароль");
            }

            // Проверяем, что введенный пароль совпадает с хешем пароля в базе данных
            if (!_service.VerifyPassword(request.Password, user.Password))
            {
                return Unauthorized("Неверный логин или пароль");
            }

            // Создаем объект Claims для пользователя
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name)
            };

            // Добавляем роль пользователя в список claims
            if (user.Role == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                user.Role = "Customer";
                claims.Add(new Claim(ClaimTypes.Role, "Customer"));
            }

            // Создаем объект ClaimsIdentity и устанавливаем его в HttpContext.User
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30), // Время жизни cookie (в данном случае 30 минут)
                IsPersistent = true
            });

            // Извлекаем имя и роль из объекта ClaimsPrincipal
            string name = principal.FindFirst(ClaimTypes.Name)?.Value;
            string role = principal.FindFirst(ClaimTypes.Role)?.Value;

            // Возвращаем успешный ответ с актуальной информацией
            return Ok(new { name, role });


        }
    }
}
