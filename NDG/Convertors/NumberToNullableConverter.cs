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
    public class NumberToNullableConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int integerNumber;
            Decimal decimalNumber;
            double doubleNumber;
            bool isNumber = int.TryParse(value as string, out integerNumber) 
                                || double.TryParse(value as string, out doubleNumber)
                                || Decimal.TryParse(value as string, out decimalNumber);
            if (!isNumber)
            {
                return null;
            }

            return value;
        }
    }
}
