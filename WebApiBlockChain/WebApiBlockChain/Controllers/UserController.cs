using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    }
}
