﻿using WebApiBlockChain.Models;
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
            result += block.Category.Tag;
            result += block.UserId;
            result += block.User.Name;
            result += block.User.Password;
            result += block.User.Role;
            result += block.PreviousHash;
            result += block.Nonce.ToString();

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
            result += block.Category.Tag;
            result += block.UserId;
            result += block.User.Name;
            result += block.User.Password;
            result += block.User.Role;
            result += block.PreviousHash;
            result += block.Nonce.ToString();

            return result;
        }

        public string GetDataHash(Block block, DateTime time)
        {
            return GetHash(GetData(block,time));
        }

        public Block MineBlock(Block block, int difficulty)
        {
            // Инициализация значения nonce блока равным 0
            block.Nonce = 0;
            // Вычисление хеша блока с использованием текущего значения nonce
            string hash = GetDataHash(block);
            // Создание строки-цели, которая состоит из 'difficulty' количества нулей
            string target = new string('0', difficulty);
            // Цикл продолжается до тех пор, пока начало хеша не будет соответствовать цели
            while (!hash.StartsWith(target))
            {
                // Увеличение nonce на 1 для изменения хеша блока
                block.Nonce++;
                // Повторное вычисление хеша блока с новым значением nonce
                hash = GetDataHash(block);
            }
            // После нахождения подходящего хеша, он присваивается свойству Hash блока
            block.Hash = hash;
            // Возвращение добытого блока с корректным хешем
            return block;
        }
        //public bool IsBlockchainValid(List<Block> blockchain)
        //{
        //    for (int i = 1; i < blockchain.Count; i++)
        //    {
        //        Block currentBlock = blockchain[i];
        //        Block previousBlock = blockchain[i - 1];

        //        if (currentBlock.PreviousHash != GetDataHash(previousBlock))
        //        {
        //            Console.WriteLine($"Ошибка: Хеш предыдущего блока {previousBlock.Id} не соответствует хешу в блоке {currentBlock.Id}");
        //            return false;
        //        }
        //    }

        //    return true;
        //}
        public bool VerifyPassword(string requestUserPassword , string dbUserPassword)
        {
            if (GetHash(requestUserPassword) == dbUserPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
                
              
        }
    }
}
