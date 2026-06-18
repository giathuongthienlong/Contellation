using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    /// <summary>
    /// Converter chuyển null thành Collapsed, ngược lại thành Visible.
    /// Dùng cho Icon, Button Clear, v.v.
    /// </summary>
    public class NullToCollapsedConverter : IValueConverter
    {
        public static readonly NullToCollapsedConverter Instance = new NullToCollapsedConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Nếu value là null → Collapsed
            if (value == null)
                return Visibility.Collapsed;

            // Nếu là string rỗng → Collapsed (tùy chọn)
            if (value is string str && string.IsNullOrWhiteSpace(str))
                return Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
