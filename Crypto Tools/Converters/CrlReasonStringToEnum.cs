using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using CryptoTools.Helpers;

namespace CryptoTools.Converters
{
    class CrlReasonStringToEnum : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Enum.GetName(typeof (CrlReasonEx), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? (int)CrlReasonEx.Unspecified : (int)Enum.Parse(typeof (CrlReasonEx), value.ToString());
        }
    }
}
