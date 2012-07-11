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
    /// <summary>
    /// Converter which changes full server address (with name of servlets and protocol) to short URL (containing only IP address and port number) and converts back from short URL to full address.
    /// </summary>
    public class ServerUrlDisplayConverter : IValueConverter
    {
        /// <summary>
        /// Changes URL address from full address to short address by removing protocol and name of servlet.
        /// </summary>
        /// <param name="value">Full URL address (with protocol and servlet name).</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Returns short URL address (only IP address [or DNS] and port number.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //string output = string.Empty;
            //if (value != null)
            //{
            //    //output = value.ToString().Replace("http://", "").Replace("https://", "");
            //   // output = value.ToString().TrimEnd('/');
            //}
            return value;
        }

        /// <summary>
        /// Changes URL address from short address to full address by adding protocol and name of servlet.
        /// </summary>
        /// <param name="value">Short URL address (only IP address and port number).</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Returns full URL address (with protocol and servlet name).</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string output = value.ToString();
            //if (!output.StartsWith("http://") && !output.StartsWith("https://"))
            //{
            //    output = string.Concat("http://", output);
            //}
            //if (!output.EndsWith("/"))
            //{
            //    output = string.Concat(output,"/");
            //}
            return output;
        }
    }
}
