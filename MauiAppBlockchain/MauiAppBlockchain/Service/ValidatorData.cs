using MauiAppBlockchain.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static MauiAppBlockchain.MainPage;

namespace MauiAppBlockchain.Service
{
    public class ValidatorData
    {
        public static bool ValidateCheckData(Block block,Label label)
        {
            var context = new ValidationContext(block);
            var results = new List<ValidationResult>();
            label.BackgroundColor = Colors.Black;

            if (!Validator.TryValidateObject(block, context, results, true))
            {
                DisplayArtService.PrintLabelStatus(label, "Не удалось создать объект Block", LabelStatus.Error);
                foreach (var error in results)
                {           
                    label.Text += $"\n{error.ErrorMessage}";
                }           
                return false;
            }
            else
            {
                DisplayArtService.PrintLabelStatus(label, "Объект Block успешно сформирован", LabelStatus.Success);                
                return true;
            }

                
        }
    }
}
