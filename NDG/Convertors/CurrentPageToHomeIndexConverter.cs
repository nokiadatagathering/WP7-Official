// ----------------------------------------------------------------------
// <copyright file="CurrentPageToHomeIndexConverter.cs" company="QArea">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace NDG.Convertors
{
    using System;
    using System.Windows.Data;
    using NDG.ViewModels;

    /// <summary>
    /// Converts current page index to home page index.
    /// </summary>
    public class CurrentPageToHomeIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (HomePageIndexes)value;
        }
    }
}
