using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace WebApiBlockChain.Models
{
    public class Block
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; } 
        public decimal Amount { get; set; }
        //[Required]
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public string? Hash { get; set; }
        public string? PreviousHash { get; set; }
        public int Nonce { get; set; }

           
    }
}
