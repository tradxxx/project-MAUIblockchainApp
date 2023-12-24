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

        public IEnumerable<User> GetUsers(Func<User, bool> predicate) =>
          _db.Users.Where(predicate).ToArray();
        public IEnumerable<User> GetUsers() =>
            GetUsers((x) => true);
        public IEnumerable<Category> GetCategories(Func<Category, bool> predicate) =>
          _db.Categories.Where(predicate).ToArray();
        public IEnumerable<Category> GetCategories() =>
            GetCategories((x) => true);

        public void Add(Block block)
		{
			_db.Blocks.Add(block);
			_db.SaveChanges();
		}
	}
}
