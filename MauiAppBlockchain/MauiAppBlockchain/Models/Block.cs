using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppBlockchain.Models
{
    public class Block
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Id { get; set; }
        [Required(ErrorMessage = "Please enter your name")]
        public string Data { get; set; }
        public DateTime Created { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        [Required(ErrorMessage = "Please enter your user")]
        public string User { get; set; }

    }
}
