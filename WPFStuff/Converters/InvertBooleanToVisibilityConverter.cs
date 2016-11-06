using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFStuff.Converters
{
    public class InvertBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanVisibility = value as bool?;

            if (booleanVisibility == null) return Visibility.Collapsed;

            return (bool)booleanVisibility ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
