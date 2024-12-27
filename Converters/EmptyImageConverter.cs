using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Product_Manager.Converters
{
    internal class EmptyImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert Function
        /// ----------------
        /// 1. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>
        ///  1. 
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageString = (string)value;

            if (imageString == null)
            {
                var defaultImage = new BitmapImage(new Uri("pack://application:,,,/Assets (Solution)/Images/Product.jpg"));
                return defaultImage;
            }
            byte[] imageBytes = System.Convert.FromBase64String(imageString);

            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
