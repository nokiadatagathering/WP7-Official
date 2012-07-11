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
using System.Collections.ObjectModel;
using NDG.DataAccessModels.DataModels;

namespace NDG.Convertors
{
    public class MultipleChoiceQuestionConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var selectedItems = value as ObservableCollection<object>;
            var result = new ObservableCollection<ChoiceTextValuePair>();
            if (selectedItems != null)
            {
                foreach (var item in selectedItems)
                {
                    result.Add(item as ChoiceTextValuePair);
                }
            }

            return result;
        }
    }
}
