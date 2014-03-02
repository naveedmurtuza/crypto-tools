using System;
using System.Linq;
using System.Windows.Data;

namespace CryptoTools.Converters
{
    /// <summary>
    /// A Multi Value converter for converting command parameters on System KeyStore  menu item.
    /// 
    /// </summary>
    public class X509StoreNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.ToArray();// new String[] { values[0].ToString(), values[1].ToString()};
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
