using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAppBlockchain.Models
{
    public class Category
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("icon")]
        public string? Icon { get; set; }
        [JsonIgnore]
        public virtual ICollection<Block>? CategoryBlocks { get; set; }
    }
}
