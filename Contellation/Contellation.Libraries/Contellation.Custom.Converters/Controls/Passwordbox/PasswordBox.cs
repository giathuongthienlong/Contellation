using System.Globalization;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    /// <summary>
    /// Converter đổi icon giữa hiện và ẩn mật khẩu
    /// </summary>
    public class RevealIconConverter : IValueConverter
    {
        public static readonly RevealIconConverter Instance = new RevealIconConverter();

        /// <summary>
        /// True = đang hiện mật khẩu → trả về icon "ẩn" (🙈)
        /// False = đang ẩn mật khẩu → trả về icon "hiện" (👁)
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isRevealed)
            {
                return isRevealed ? "🙈" : "👁";   // Có thể thay bằng Unicode hoặc Path
            }

            return "👁"; // mặc định
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
