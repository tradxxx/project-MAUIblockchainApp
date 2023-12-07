using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApiBlockChain.Data;
using WebApiBlockChain.Models;

namespace WebApiBlockChain
{
	public class EntityGateway
	{
		private readonly BlockchainContext _db;

		public EntityGateway(BlockchainContext db)
		{
			_db = db;
		}

		public IEnumerable<Block> GetBlocks(Func<Block, bool> predicate) =>
		  _db.Blocks.Where(predicate).ToArray();
		public IEnumerable<Block> GetBlocks() =>
			GetBlocks((x) => true);

		
		public void Add(Block block)
		{
			_db.Blocks.Add(block);
			_db.SaveChanges();
		}
	}
}
