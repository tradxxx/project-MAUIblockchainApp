using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiBlockChain.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        //[Required(ErrorMessage = "Please enter your data")]
        public string Description { get; set; }

        public virtual Category? Category { get; set; }

        //[Required(ErrorMessage = "Please enter your user")]
        public virtual User? User { get; set; }
    }

    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string? Icon { get; set; }

        [JsonIgnore]
        public virtual List<Expense>? CategorysExpenses { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [JsonIgnore]
        public virtual List<Expense>? UsersExpenses { get; set; }
    }
}
