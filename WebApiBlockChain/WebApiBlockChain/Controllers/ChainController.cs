using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Threading.Tasks.Dataflow;
using WebApiBlockChain.Models;
using WebApiBlockChain.Service;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebApiBlockChain.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChainController : ControllerBase
	{
		private readonly EntityGateway _db;
		private readonly IBlockService _sevice;

        public static Chain chain;

        private ICollection<Block> ruinblocks = new List<Block>();

		
		public ChainController(EntityGateway db, IBlockService service)
		{
			_db = db;
			_sevice = service;
            chain = new Chain(service);
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
				last_time = blocks.Last().Date;
			}

			return Ok(new
			{
				status = "ok",
				blocks_count = blocks.Count(),
				last_time_block = last_time,
				blocks = blocks
			});
		}

        [Authorize]
        [HttpPost]
		public IActionResult CreateBlock(Block item)
		{
            if (ModelState.IsValid)
			{
				//Синхронизация с клиентом и проверка BlockChain
				chain.Blocks = _db.GetBlocks().ToList<Block>();

				if (chain.Blocks.Count() == 0)
				{
					var genesisBlock = new Block() 
					{
                        Amount = 0,
						Description = "Генезис",
						Date = DateTime.Now,
						Category = new Category() { Title = "Генезис", Icon = "Генезис" },
						User = new User() { Name = "Admin", Password = _sevice.GetHash("root")},
						Nonce = 0
                    };

                    genesisBlock.PreviousHash = "1";
                    genesisBlock.Hash = _sevice.GetDataHash(genesisBlock);		

					chain.Blocks.Add(genesisBlock);
					chain.Last = genesisBlock;
					//chain.Last.Id = null;
					_db.AddBlock(chain.Last);
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

				item.PreviousHash = chain.Last.Hash;

				var user = _db.GetUsers(x=>x.Id ==item.UserId).FirstOrDefault();
				var category = _db.GetCategories(x => x.Id == item.CategoryId).FirstOrDefault();

                var newblock = _sevice.CreateBlock(item, category, user);

                //Майнинг,Сложность майнинга: 4 нуля в начале хеша
                newblock = _sevice.MineBlock(newblock, 4);

                chain.Last = newblock;			

				_db.AddBlock(newblock);

				
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
