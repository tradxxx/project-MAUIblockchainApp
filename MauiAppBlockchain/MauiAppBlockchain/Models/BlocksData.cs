using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppBlockchain.Models
{
    public class BlocksData
    {         

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("blocks_count")]
        public int BlocksCount { get; set; }

        [JsonPropertyName("last_time_block")]
        public DateTime LastTimeBlock { get; set; }

        [JsonPropertyName("blocks")]
        public IEnumerable<Block> Blocks { get; set; }
    }
}
