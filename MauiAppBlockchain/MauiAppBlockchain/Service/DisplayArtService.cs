using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppBlockchain.Service
{
    public enum LabelStatus
    {
        Success,
        Error
    }
    public static class DisplayArtService
    {
       
        public static void PrintLabelStatus(Label label, string text, LabelStatus status)
        {
            switch (status) 
            {
                case LabelStatus.Success:
                    label.Text = text;
                    label.TextColor = Color.FromRgb(0, 255, 0);
                    break;
                case LabelStatus.Error:
                    label.Text = text;
                    label.TextColor = Color.FromRgb(255, 0, 0);
                    break;
            }
        }
    }
}
