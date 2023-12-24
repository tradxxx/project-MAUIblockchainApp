using WebApiBlockChain.Service;

namespace WebApiBlockChain.Models
{
    public class Chain
    {
        private readonly IBlockService _service;

        public Chain(IBlockService service)
        {
            _service= service;
        }
        public List<Block> Blocks { get; set; }

        public Block Last { get;set; }
     
   
        public bool Check(ref ICollection<Block> ruinblocks)
        {
            //Транзакция считается защищенной, если ПОСЛЕ НЕЁ ЕСТЬ ХОТЯ БЫ ОДНА СЛЕДУЮЩАЯ ТРАНЗАКЦИЯ, ЕСЛИ False, ТО НУЖНО УДАЛИТЬ ПОСЛЕДНЮЮ ТРАНЗАКЦИЮ, ТК ПРИ ЕЁ ФОРМИРОВАНИИ ПРОИЗОШЕЛ ВЗЛОМ
            for (int i = 1; i < Blocks.Count; i++)
            {
                // blockchain[2].amount = 3454353; Изменение содержимого блока на фальшивые данные
                ;
                if (Blocks[i].Hash != _service.GetDataHash(Blocks[i], Blocks[i].Date))
                {

                    ruinblocks.Add(Blocks[i]);
                    return false;
                }
                //blockchain[2].hash = blockchain[2].getHash(); Изменение хэша блока на фальшивый хэш
                if (Blocks[i].PreviousHash != Blocks[i - 1].Hash)
                {
                    ruinblocks.Add(Blocks[i]); 
                    return false;
                }
                //Проверка связи (пересчёт хэша)
                if (Blocks[i].PreviousHash != _service.GetDataHash(Blocks[i-1], Blocks[i-1].Date))
                {
                    ruinblocks.Add(Blocks[i]);
                    return false;
                }
            }

            return true;
        }

        
    }
}
