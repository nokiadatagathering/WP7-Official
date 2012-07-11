using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace NDG.ViewModels.Helpers
{
    public class FontSizePare
    {
        public double Size { get; set; }
        public string Text { get; set; }
        public FontSizesNames Name { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public enum FontSizesNames
    {
        Small,
        Medium,
        Large,
        Custom
    }
}
