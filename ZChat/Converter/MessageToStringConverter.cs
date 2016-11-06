using System;
using System.Globalization;
using System.Windows.Data;
using ZChat.Shared;

namespace ZChat.Converter
{
    internal class MessageToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var message = value as Message;
            if (message == null)
                return String.Empty;

            string messageString;
            if (message.Sender != Constants.ServerId)
                messageString = $"[ {message.TimeStamp.ToLongTimeString()} ] <{message.Sender}> {message.Content}";
            else
                messageString = $"[ {message.TimeStamp.ToLongTimeString()} ] {message.Content}";

            return messageString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
