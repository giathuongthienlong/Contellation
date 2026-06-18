using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    public class EmptyStringToCollapsedConverter : IValueConverter
    {
        public static readonly EmptyStringToCollapsedConverter Instance = new EmptyStringToCollapsedConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
                return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;

            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
