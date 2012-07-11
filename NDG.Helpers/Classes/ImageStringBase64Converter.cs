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
using System.Windows.Media.Imaging;
using System.IO;
using System.Text;

namespace NDG.Helpers.Classes
{
    /// <summary>
    /// Helper for converting Image to Base64 String (store answers to Image questions in such format)
    /// </summary>
    public class ImageStringBase64Converter
    {
        public BitmapImage GetImageFromStringBase64(string imageString)
        {
            if (!string.IsNullOrEmpty(imageString))
                using (var stream = new MemoryStream(System.Convert.FromBase64String(imageString)))
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(stream);
                    return image;
                }
            return null;
        }
        public string GetStringBase64FromImage(BitmapImage bitmapImage, int width, int height)
        {
            if (bitmapImage != null)
                using (MemoryStream stream = new MemoryStream())
                {
                    WriteableBitmap writeable = new WriteableBitmap(bitmapImage);
                    writeable.SaveJpeg(stream, width, height, 0, 100);
                    stream.Seek(0, SeekOrigin.Begin);
                    return System.Convert.ToBase64String(stream.GetBuffer());
                }

            return null;
        }
    }
}
