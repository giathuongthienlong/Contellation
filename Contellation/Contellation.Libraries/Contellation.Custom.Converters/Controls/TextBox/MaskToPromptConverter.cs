using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Contellation.Custom.Converters.Controls
{
    /// <summary>
    /// Chuyển Mask thành dạng hiển thị prompt (ví dụ: (###) ###-#### → (___) ___-____)
    /// </summary>
    public class MaskToPromptConverter : IValueConverter
    {
        public static readonly MaskToPromptConverter Instance = new MaskToPromptConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is not string mask || string.IsNullOrEmpty(mask))
                return string.Empty;

            var result = new StringBuilder();

            foreach (char c in mask)
            {
                switch (c)
                {
                    case '#':           // Số
                    case '0':           // Số bắt buộc
                    case '9':           // Số tùy chọn
                        result.Append('_');
                        break;

                    case 'L':           // Chữ cái
                    case 'A':           // Chữ cái hoặc số
                    case 'a':           // Chữ cái tùy chọn
                        result.Append('_');
                        break;

                    case '>':           // Uppercase
                    case '<':           // Lowercase
                        break;          // Bỏ qua

                    default:            // Ký tự cố định (dấu ngoặc, dấu gạch ngang, khoảng trắng...)
                        result.Append(c);
                        break;
                }
            }

            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
