using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CryptoTools.Converters
{

    public class Base64Converter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (targetType != typeof(byte[]))
            //    throw new InvalidOperationException("The target must be a byte array: " + targetType);
            if (value == null)
                return String.Empty;
            return System.Convert.ToBase64String((byte[])value).ToLower();

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            if (value == null)
                return null;
            return System.Convert.FromBase64String((String)value);
        }
    }

}
