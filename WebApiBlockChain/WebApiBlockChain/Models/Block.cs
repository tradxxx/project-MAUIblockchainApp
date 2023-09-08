using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace WebApiBlockChain.Models
{
    public class Block
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [Required(ErrorMessage ="Please enter your name")]
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        [Required(ErrorMessage = "Please enter your user")]
        public string User { get; set; }


        public Block()
        {
            //Id = 1;
            Data = "Hello, World";
            Created = DateTime.Parse("07.02.2023 00:00:00.0");
            PreviousHash = "111111";
            User = "Admin";

            var source = GetData();
            Hash = GetHash(source);
        }

        public Block(string data, string user, Block block)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException("Пустой аргумент data", nameof(data));
            }
            if (block == null)
            {
                throw new ArgumentNullException("Пустой аргумент block", nameof(block));
            }
            if (string.IsNullOrWhiteSpace(user))
            {
                throw new ArgumentNullException("Пустой аргумент user", nameof(user));
            }

            Data = data;
            User = user;
            PreviousHash = block.Hash;
            Created = DateTime.Now;
            //Id = block.Id + 1;

            var source = GetData();
            Hash = GetHash(source);
        }

        public string GetData()
        {
            string result = "";

            result += Data;
            //Нельзя использовать тк при проверке хэшей в реальном времени в Функции Check генерирется текущее время, а не время создания блока, что приводит к несовпадению хэшей
            result += Created.ToString("dd.MM.yyyy HH:mm:ss.f");
            result += PreviousHash;
            result += User;

            return result;
        }
        //Метод для функции Сheck
        public string GetData(DateTime time)
        {
            string result = "";

            result += Data;
            //Нельзя использовать тк при проверке хэшей в реальном времени в Функции Check генерирется текущее время, а не время создания блока, что приводит к несовпадению хэшей
            result += time.ToString("dd.MM.yyyy HH:mm:ss.f");
            result += PreviousHash;
            result += User;

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

        public override string ToString()
        {
            return Data + " / " + Created;
        }
    }
}
