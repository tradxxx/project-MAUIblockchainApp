using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;

namespace WebApiBlockChain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlockController : ControllerBase
    {
        private readonly BlockchainContext _dbContext;

        public BlockController(BlockchainContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Получение списка всех блоков
        [HttpGet]
        public ActionResult<IEnumerable<Block>> GetBlocks()
        {
            var blocks = _dbContext.Blocks.ToList();
            return Ok(blocks);
        }

        // Создание нового блока
        [HttpPost]
        public ActionResult<Block> CreateBlock([FromBody] Block block)
        {
            if (block == null)
            {
                return BadRequest("Invalid block data");
            }

            _dbContext.Blocks.Add(block);
            _dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetBlocks), new { id = block.Id }, block);
        }
    }
}
