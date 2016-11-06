using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WPFStuff.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanVisibility = value as bool?;

            if (booleanVisibility == null) return Visibility.Collapsed;

            return (bool)booleanVisibility ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
