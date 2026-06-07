using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    // So sánh số
    public class LessThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new LessThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;

            try
            {
                double doubleValue = System.Convert.ToDouble(value);
                double compareTo = System.Convert.ToDouble(parameter);
                return doubleValue < compareTo;
            }
            catch
            {
                return false; // an toàn hơn
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    public class GreaterThanConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new GreaterThanConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) return false;

            try
            {
                double doubleValue = System.Convert.ToDouble(value);
                double compareTo = System.Convert.ToDouble(parameter);
                return doubleValue > compareTo;
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }

    // Converter tiện lợi cho Responsive (kết hợp với BreakPoints)
    public class ResponsiveVisibilityConverter : IValueConverter
    {
        public static readonly IValueConverter Instance = new ResponsiveVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not double width || parameter is not string param)
                return Visibility.Collapsed;

            // parameter ví dụ: "SM" hoặc "MD,768"
            var parts = param.Split(',');
            string bpName = parts[0].Trim().ToUpper();
            int minWidth = parts.Length > 1 ? int.Parse(parts[1]) : GetDefaultBreakPoint(bpName);

            return width >= minWidth ? Visibility.Visible : Visibility.Collapsed;
        }

        private int GetDefaultBreakPoint(string bp)
        {
            return bp switch
            {
                "SM" => 576,
                "MD" => 768,
                "LG" => 992,
                "XL" => 1200,
                _ => 0
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
