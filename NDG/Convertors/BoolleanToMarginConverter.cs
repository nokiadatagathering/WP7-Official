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
using System.Windows.Data;

namespace NDG.Convertors
{
    public class BoolleanToMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isVisible = (bool)value;
            Thickness result = new Thickness(0);
            if (isVisible && parameter != null)
            {
                string thickness = (string)parameter;
                var margins = thickness.Split(',');
                result = new Thickness(double.Parse(margins[0]), double.Parse(margins[1]), double.Parse(margins[2]), double.Parse(margins[3]));
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
