using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Product_Manager.Converters
{
    internal class EmptyImageConverter : IValueConverter
    {
        /// <summary>
        /// Convert Function
        /// ----------------
        /// 1. Converts Base64String stored in JsonRepository to BitmapImage type for View.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>
        ///     1. returns a default asset image converted to BitmapImage type if value is null from ViewModel.
        ///     2. returns the Base64String converted to BitmapImage by assigning MemoryStream object to StreamSource.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imageString = (string)value;

            if (imageString == null)
            {
                BitmapImage defaultImage = new BitmapImage(new Uri("pack://application:,,,/Assets (Solution)/Images/Product.jpg"));
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

        /// <summary>
        /// ConvertBack Function
        /// --------------------
        /// 1. Not Implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
