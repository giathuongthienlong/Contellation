using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// CurrencyBox chuyên dùng để nhập số tiền, tự động format theo culture.
    /// </summary>
    //[ContentProperty(nameof(Number))]
    public class CurrencyBox : TextBox
    {
        #region Dependency Properties

        public decimal? Value
        {
            get => (decimal?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(decimal?), typeof(CurrencyBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public string CurrencySymbol
        {
            get => (string)GetValue(CurrencySymbolProperty);
            set => SetValue(CurrencySymbolProperty, value);
        }
        public static readonly DependencyProperty CurrencySymbolProperty =
            DependencyProperty.Register(nameof(CurrencySymbol), typeof(string), typeof(CurrencyBox),
                new PropertyMetadata("₫", OnFormatChanged));

        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }
        public static readonly DependencyProperty DecimalPlacesProperty =
            DependencyProperty.Register(nameof(DecimalPlaces), typeof(int), typeof(CurrencyBox),
                new PropertyMetadata(0, OnFormatChanged));

        public bool AllowNegative
        {
            get => (bool)GetValue(AllowNegativeProperty);
            set => SetValue(AllowNegativeProperty, value);
        }
        public static readonly DependencyProperty AllowNegativeProperty =
            DependencyProperty.Register(nameof(AllowNegative), typeof(bool), typeof(CurrencyBox),
                new PropertyMetadata(false));

        public bool ClearButtonEnabled
        {
            get => (bool)GetValue(ClearButtonEnabledProperty);
            set => SetValue(ClearButtonEnabledProperty, value);
        }
        public static readonly DependencyProperty ClearButtonEnabledProperty =
            DependencyProperty.Register(nameof(ClearButtonEnabled), typeof(bool), typeof(CurrencyBox),
                new PropertyMetadata(true));

        #endregion

        #region Constructor

        #endregion

        static CurrencyBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CurrencyBox),
                new FrameworkPropertyMetadata(typeof(CurrencyBox)));
        }

        public CurrencyBox()
        {
            HorizontalContentAlignment = HorizontalAlignment.Right;
            Loaded += CurrencyBox_Loaded;
        }

        private void CurrencyBox_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDisplayText();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CurrencyBox box)
                box.UpdateDisplayText();
        }

        private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CurrencyBox box)
                box.UpdateDisplayText();
        }

        private void UpdateDisplayText()
        {
            if (Value.HasValue)
            {
                string format = DecimalPlaces > 0 ? $"N{DecimalPlaces}" : "N0";
                string formatted =  Value.Value.ToString(format, CultureInfo.CurrentCulture);

                // Thêm Currency Symbol (nếu có)
                if (!string.IsNullOrEmpty(CurrencySymbol))
                    Text = $"{formatted} {CurrencySymbol}";
                else
                    Text = formatted;
            }
            else
            {
                Text = string.Empty;
            }
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            ParseAndUpdateValue();
        }

        private void ParseAndUpdateValue()
        {
            string input = Text.Trim();

            if (string.IsNullOrEmpty(input))
            {
                SetCurrentValue(ValueProperty, null);
                return;
            }

            // Loại bỏ symbol tiền tệ và khoảng trắng
            // Loại bỏ symbol và ký tự không phải số
            string cleanInput = new string(input.Where(c => char.IsDigit(c) || c == '.' || c == ',' || c == '-').ToArray());

            if (decimal.TryParse(cleanInput, NumberStyles.Any, CultureInfo.CurrentCulture, out decimal result))
            {
                if (!AllowNegative && result < 0)
                    result = Math.Abs(result);

                SetCurrentValue(ValueProperty, result);
                UpdateDisplayText();
            }
            else
            {
                UpdateDisplayText(); // Khôi phục giá trị cũ
            }
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            // Chỉ cho phép số, dấu phẩy, dấu chấm, dấu trừ (nếu AllowNegative)
            string allowed = AllowNegative ? "0123456789.,-" : "0123456789.,";

            if (!allowed.Contains(e.Text))
            {
                e.Handled = true;
            }
            base.OnPreviewTextInput(e);
        }
    }
}
