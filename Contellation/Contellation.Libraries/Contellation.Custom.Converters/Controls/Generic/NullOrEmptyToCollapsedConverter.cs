using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    public class NullOrEmptyToCollapsedConverter : IValueConverter
    {
        public static readonly NullOrEmptyToCollapsedConverter Instance = new NullOrEmptyToCollapsedConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Visibility.Collapsed;

            if (value is string str)
                return string.IsNullOrWhiteSpace(str) ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
