using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using WebApiBlockChain.Models;

namespace WebApiBlockChain.Data
{
    public class BlockchainContext : DbContext
    {

        public DbSet<Block> Blocks { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public BlockchainContext(DbContextOptions<BlockchainContext> options) : base(options)
        {

        }
        
       


    }
}
