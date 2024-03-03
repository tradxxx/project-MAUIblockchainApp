using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;
using WebApiBlockChain.Service;

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
                 new Claim(ClaimTypes.Name, user.Name),
                 new Claim(ClaimTypes.Role, "Пользователь")
            };

            // Создаем объект ClaimsIdentity и устанавливаем его в HttpContext.User
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30), // Время жизни cookie (в данном случае 30 минут)
                IsPersistent = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            // Возвращаем успешный ответ
            return Ok(new
            {
                name = User.FindFirst(ClaimTypes.Name)?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }




    }
}
