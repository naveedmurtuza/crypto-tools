using System;
using System.Windows.Data;
using Org.BouncyCastle.X509;

namespace CryptoTools.Converters
{
    /// <summary>
    /// Value Converter, converts Org.BouncyCastle.X509.X509Certificate object to
    /// X509Certificate.SubjectDN
    /// </summary>
    public class X509CertificateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? String.Empty : ((X509Certificate)value).SubjectDN.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
