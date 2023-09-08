using Microsoft.EntityFrameworkCore;
using System.Security.Principal;

namespace WebApiBlockChain.Models
{
    public class BlockchainContext : DbContext
    {
		

		public BlockchainContext(DbContextOptions<BlockchainContext> options) : base(options)
        {
            
        }

        public DbSet<Block> Blocks { get; set; }

		
	}
}
