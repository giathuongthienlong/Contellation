using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Contellation.Custom.Converters.Controls
{
    [ValueConversion(typeof(SolidColorBrush), typeof(Color))]
    public class SolidBrushToColorConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new SolidBrushToColorConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SolidColorBrush result)) return null;
            return result.Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
