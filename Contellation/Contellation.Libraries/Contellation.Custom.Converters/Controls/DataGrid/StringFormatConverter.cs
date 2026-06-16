using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    public class StringFormatConverter : IValueConverter, IMultiValueConverter
    {
        public static readonly IValueConverter Instance = new StringFormatConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{value}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                // values [0] contains the format
                if (values[0] == DependencyProperty.UnsetValue || string.IsNullOrEmpty(values[0]?.ToString()))
                    return string.Empty;

                var stringFormat = values[0].ToString() ?? string.Empty;

                // the last item of values array is culture
                if (parameter != null && parameter.Equals("Culture"))
                    culture = values.LastOrDefault() != null ? (CultureInfo)values.Last() : culture;

                return string.Format(culture, stringFormat, values.Skip(1).ToArray());
            }
            catch (FormatException ex)
            {
                Debug.WriteLine($"StringFormatConverter.Convert error: {ex.Message}");
                return string.Empty;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
