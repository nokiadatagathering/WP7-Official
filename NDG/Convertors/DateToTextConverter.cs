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
using System.Globalization;

namespace NDG.Convertors
{

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static bool IsDayOfCurrentWeek(this DateTime dt)
        {
            bool result = false;
            var firstDayOfWeek = CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek;
            DateTime startOfWeek = DateTime.Now.StartOfWeek(firstDayOfWeek);
            if (DateTime.Now >= startOfWeek && DateTime.Now < startOfWeek.AddDays(7))
            {
                result = true;
            }

            return result;
        }
    }


    public class DateToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var downloadedDate = (DateTime)value;
            string result = string.Empty;
            //if (DateTime.Now.Date == downloadedDate.Date)
            //{
            //    result = downloadedDate.ToString("h:mm tt");
            //}
            ////else
            ////    if (DateTime.Now.IsDayOfCurrentWeek())
            ////    {
            ////        result = downloadedDate.ToString("ddd");
            ////    }
            //else
            //{
                result = downloadedDate.ToString("MM/dd/yyyy");
           // }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
