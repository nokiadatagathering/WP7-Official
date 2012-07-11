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
using NDG.BussinesLogic.Providers;

namespace NDG.Convertors
{
    public class OffOnTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool currentValue = (bool)value;
            return currentValue ? (Application.Current.Resources["LanguageStrings"] as LanguageStrings).ON.ToLower()
                : (Application.Current.Resources["LanguageStrings"] as LanguageStrings).OFF.ToLower();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
