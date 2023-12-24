using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;

namespace WebApiBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BlockchainContext _dbContext;
        public CategoryController(BlockchainContext db) { _dbContext = db; }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _dbContext.Categories.ToList();
            return Ok(categories);
        }

        // Создание новой категории
        [HttpPost]
        public ActionResult<Category> CreateCategory([FromBody] Category category)
        {
            if (category == null)
            {
                return BadRequest("Invalid category data");
            }

            _dbContext.Categories.Add(category);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
        }
    }
}
