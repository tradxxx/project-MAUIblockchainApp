using WebApiBlockChain.Models;
using XSystem.Security.Cryptography;

namespace WebApiBlockChain.Service
{
    public class BlockService : IBlockService
    {
        public Block CreateBlock(Block block, Category category, User user)
        {
            block.Category = category;
            block.User = user;
            block.Hash = GetDataHash(block);
            return block;
        }

        public string GetData(Block block)
        {
            string result = "";
           
            result += block.Amount.ToString("F2");
            result += block.Description;
            //Нельзя использовать тк при проверке хэшей в реальном времени в Функции Check генерирется текущее время, а не время создания блока, что приводит к несовпадению хэшей
            result += block.Date.ToString("dd.MM.yyyy HH:mm:ss.f");
            result += block.CategoryId;
            result += block.Category.Title;
            result += block.Category.Icon;
            result += block.UserId;
            result += block.User.Name;
            result += block.User.Password;
            result += block.PreviousHash;

            return result;
        }
       

        public string GetHash(string data)
        {
            var message = System.Text.Encoding.ASCII.GetBytes(data);
            var hashString = new SHA256Managed();
            string hex = "";

            var hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public string GetDataHash(Block block)
        {         
            return GetHash(GetData(block));
        }


        //For ChainCheck
        public string GetData(Block block, DateTime time)
        {
            string result = "";

            result += block.Amount.ToString("F2");
            result += block.Description;
            //Нельзя использовать тк при проверке хэшей в реальном времени в Функции Check генерирется текущее время, а не время создания блока, что приводит к несовпадению хэшей
            result += time.ToString("dd.MM.yyyy HH:mm:ss.f");
            result += block.CategoryId;
            result += block.Category.Title;
            result += block.Category.Icon;
            result += block.UserId;
            result += block.User.Name;
            result += block.User.Password;
            result += block.PreviousHash;

            return result;
        }

        public string GetDataHash(Block block, DateTime time)
        {
            return GetHash(GetData(block,time));
        }


    }
}
