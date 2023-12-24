using WebApiBlockChain.Models;
using XSystem.Security.Cryptography;

namespace WebApiBlockChain.Service
{
    public interface IBlockService
    {
        Block CreateBlock(Block block,Category category,User user);
        string GetHash(string data);
        string GetData(Block block);
        string GetDataHash(Block block);
        string GetData(Block block, DateTime time);
        string GetDataHash(Block block, DateTime time);


    }
}
