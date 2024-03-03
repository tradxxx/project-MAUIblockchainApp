using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;
using WebApiBlockChain.Service;

namespace WebApiBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly EntityGateway _db;
        private readonly IBlockService _service;
        public CategoryController(EntityGateway db, IBlockService service)
        { 
            _db = db; 
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _db.GetCategories();
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

            _db.AddCategory(category);

            return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
        }
    }
}
