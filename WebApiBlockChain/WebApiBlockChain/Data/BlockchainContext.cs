using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using WebApiBlockChain.Models;

namespace WebApiBlockChain.Data
{
    public class BlockchainContext : DbContext
    {

        public DbSet<Block> Blocks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }

        public BlockchainContext(DbContextOptions<BlockchainContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseLazyLoadingProxies();


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
              

            modelBuilder.Entity<Block>()
                .HasOne(block => block.Category)
                .WithMany(category => category.CategoryBlocks)
                .HasForeignKey(block => block.CategoryId);

            modelBuilder.Entity<Block>()
                .HasOne(block => block.User)
                .WithMany(user => user.UserBlocks)
                .HasForeignKey(block => block.UserId);
        }
    }
}
