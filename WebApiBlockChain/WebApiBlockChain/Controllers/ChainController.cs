using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks.Dataflow;
using WebApiBlockChain.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApiBlockChain.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChainController : ControllerBase
	{
		//private readonly BlockchainContext _db;

		//public ChainController(BlockchainContext context)
		//{
		//	_db = context;
		//}

		//[HttpGet]
		//public IActionResult Get()
		//{
		//	return new ObjectResult( _db.Blocks.ToList());
		//}


		private readonly EntityGateway _db;

		private ICollection<Block> ruinblocks = new List<Block>();

        public static Chain chain = new Chain();
		public ChainController(EntityGateway db)
		{
			_db = db;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var blocks = _db.GetBlocks();
			DateTime last_time;

			if (blocks.FirstOrDefault() == null)
			{
				last_time = DateTime.MinValue;
			}
			else
			{
				last_time = blocks.Last().Created;
			}

			return Ok(new
			{
				status="ok",
				blocks_count = blocks.Count(),
				last_time_block = last_time,
				blocks = blocks
			});
		}


		[HttpPost]
		public IActionResult CreateBlock(Block item)
		{
            if (ModelState.IsValid)
			{
				//Синхронизация с клиентом и проверка BlockChain
				chain.Blocks = _db.GetBlocks().ToList<Block>();

				if (chain.Blocks.Count() == 0)
				{
					var genesisBlock = new Block();

					chain.Blocks.Add(genesisBlock);
					chain.Last = genesisBlock;
					//chain.Last.Id = null;
					_db.Add(chain.Last);
				}
				else
				{
					try
					{
                        if (chain.Check(ref ruinblocks))
                        {
                            chain.Last = chain.Blocks.Last();
                        }
                        else
                        {

                            throw new Exception("Ошибка в получении блоков из базы данных");
                        }
                    }
					catch (Exception) 
					{
						return BadRequest(new
						{
							status = "ruin blockchain",
                            blocks_count = ruinblocks.Count(),
                            last_time_block = DateTime.Now,
                            blocks = ruinblocks
                        }) ;
					}
					
				}




				if (item == null)
				{
					return BadRequest(new
					{
						context = "item is null"
					});
				}
				Block block = new Block(item.Data, item.User, chain.Last);
				chain.Last = block;
				//block.Id = null;

				_db.Add(block);

				//var blocks = _context.Blocks;
				return Ok(new
				{
					status="Okey"
				});
			}
			else
			{
				return BadRequest(new
				{
					context = "model is not valid"
				});
			}

		}
	}
}
