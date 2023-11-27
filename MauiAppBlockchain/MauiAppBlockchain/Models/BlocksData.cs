using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppBlockchain.Models
{
    public class BlocksData
    {
        public string Status { get; set; }
        public int Blocks_count { get; set; }
        public DateTime Last_time_block { get; set; }
        public IEnumerable<Block> Blocks { get; set; }
    }
}
