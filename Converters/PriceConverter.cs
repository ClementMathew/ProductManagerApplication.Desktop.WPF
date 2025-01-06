using System;
using System.Globalization;
using System.Windows.Data;

namespace Product_Manager.Converters
{
    internal class PriceConverter : IValueConverter
    {
        /// <summary>
        /// Convert Function
        /// ----------------
        /// 1. Converts price to string with $ symbol appended from view model.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>
        ///     1. returns price as string with $ symbol appended.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int priceInt = (int)value;
            string formattedPrice = string.Format("${0}", priceInt.ToString());

            return formattedPrice;
        }

        /// <summary>
        /// ConvertBack Function
        /// --------------------
        /// 1. Not implemented.
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
