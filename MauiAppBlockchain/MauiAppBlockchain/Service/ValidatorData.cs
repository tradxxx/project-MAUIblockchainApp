using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.System;
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
                label.TextColor = Color.FromRgb(255, 0, 0); 
                label.Text = "Не удалось создать объект Block";
                foreach (var error in results)
                {           
                    label.Text += $"\n{error.ErrorMessage}";
                }           
                return false;
            }
            else
            {
                label.TextColor = Color.FromRgb(0, 255, 0);
                label.Text = ($"Объект Block успешно сформирован");
                return true;
            }

                
        }
    }
}
