using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;

namespace WebApiBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BlockchainContext _dbContext;

        public UserController(BlockchainContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Получение списка всех пользователей
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _dbContext.Users.ToList();
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

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }
    }
}
