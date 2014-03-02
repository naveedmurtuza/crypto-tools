using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CryptoTools.Converters
{
    internal class MessageBoxButtonConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MessageBoxButton)) return value;

            var messageBoxButton = (MessageBoxButton) value;
            String s = parameter.ToString().ToLower();
            if (targetType == typeof (Visibility))
            {
                switch (messageBoxButton)
                {
                    case MessageBoxButton.OK:
                        if (s == "button1")
                            return Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNo:
                    case MessageBoxButton.OKCancel:
                        if (s == "button1" || s == "button2")
                            return Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
            switch (messageBoxButton)
            {
                case MessageBoxButton.OK:
                    if (s == "button1")
                        return "OK";
                    break;
                case MessageBoxButton.YesNo:
                    if (s == "button1")
                    {
                        return "Yes";
                    }
                    if (s == "button2")
                        return "No";
                    break;
                case MessageBoxButton.OKCancel:
                    if (s == "button1")
                    {
                        return "OK";
                    }
                    if (s == "button2")
                        return "Cancel";
                    break;
                case MessageBoxButton.YesNoCancel:
                    if (s == "button1")
                    {
                        return "Yes";
                    }
                    if (s == "button2")
                        return "No";
                    if (s == "button3")
                        return "Cancel";
                    break;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class MessageBoxImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var messageBoxImage = (MessageBoxImage) value;
            switch (messageBoxImage)
            {
                    case MessageBoxImage.Warning:
                    return "pack://application:,,,/Images/warning.png";
                    case MessageBoxImage.Error:
                    return "pack://application:,,,/Images/close.png";
                    case MessageBoxImage.Information:
                    return "pack://application:,,,/Images/Info.png";
                    case MessageBoxImage.Question:
                    return "pack://application:,,,/Images/help.png";
                    case MessageBoxImage.None:
                    return "";
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    internal class NotNullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.IsNullOrEmpty((String)value) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}