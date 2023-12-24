using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiBlockChain.Models
{
    
    public class Category
    {
        public long Id { get; set; }
        //[Required]
        public string Title { get; set; } 

        public string? Icon { get; set; }

        [JsonIgnore]
        public virtual ICollection<Block>? CategoryBlocks { get; set; }
    }

    public class User
    {
        public long Id { get; set; }
        //[Required]
        public string? Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Block>? UserBlocks { get; set; }
    }
}
