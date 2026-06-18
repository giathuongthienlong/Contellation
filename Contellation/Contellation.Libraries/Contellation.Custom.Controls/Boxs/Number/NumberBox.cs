using Contellation.Custom.Controls.Boxs.Number;
using Contellation.Custom.Enums.Control;
using Contellation.Custom.Extensions.Inputs;
using Contellation.Custom.Interfaces.Control.Number;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// NumberBox chuyên dụng để nhập số, hỗ trợ spin, min/max, 
    /// format và các tính năng UI hiện đại.
    /// </summary>
    public class NumberBox : TextBox
    {
        private bool _valueUpdating;

        #region Dependency Properties

        public double? Value
        {
            get => (double?)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), 
            typeof(double?), typeof(NumberBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                OnValueChanged, null, false, UpdateSourceTrigger.LostFocus));

        public int DecimalPlaces
        {
            get => (int)GetValue(DecimalPlacesProperty);
            set => SetValue(DecimalPlacesProperty, value);
        }
        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register(nameof(DecimalPlaces), 
            typeof(int), typeof(NumberBox), new PropertyMetadata(0, OnFormatChanged));

        public string CurrencySymbol
        {
            get => (string)GetValue(CurrencySymbolProperty);
            set => SetValue(CurrencySymbolProperty, value);
        }
        public static readonly DependencyProperty CurrencySymbolProperty = DependencyProperty.Register(nameof(CurrencySymbol), 
            typeof(string), typeof(NumberBox), new PropertyMetadata(string.Empty, OnFormatChanged));

        public double SmallChange
        {
            get => (double)GetValue(SmallChangeProperty);
            set => SetValue(SmallChangeProperty, value);
        }
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register(nameof(SmallChange), typeof(double),
            typeof(NumberBox), new PropertyMetadata(1.0d));

        public double LargeChange
        {
            get => (double)GetValue(LargeChangeProperty);
            set => SetValue(LargeChangeProperty, value);
        }
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register(nameof(LargeChange), typeof(double),
            typeof(NumberBox), new PropertyMetadata(10.0d));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double),
            typeof(NumberBox), new PropertyMetadata(double.MinValue));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double),
            typeof(NumberBox), new PropertyMetadata(double.MaxValue));

        public bool AcceptsExpression
        {
            get => (bool)GetValue(AcceptsExpressionProperty);
            set => SetValue(AcceptsExpressionProperty, value);
        }
        public static readonly DependencyProperty AcceptsExpressionProperty = DependencyProperty.Register(nameof(AcceptsExpression), typeof(bool),
            typeof(NumberBox), new PropertyMetadata(true));

        public NumberBoxSpinButtonPlacementMode SpinButtonPlacementMode
        {
            get => (NumberBoxSpinButtonPlacementMode)GetValue(SpinButtonPlacementModeProperty);
            set => SetValue(SpinButtonPlacementModeProperty, value);
        }
        public static readonly DependencyProperty SpinButtonPlacementModeProperty = DependencyProperty.Register(nameof(SpinButtonPlacementMode),
            typeof(NumberBoxSpinButtonPlacementMode), typeof(NumberBox), new PropertyMetadata(NumberBoxSpinButtonPlacementMode.Inline));

        public NumberBoxValidationMode ValidationMode
        {
            get => (NumberBoxValidationMode)GetValue(ValidationModeProperty);
            set => SetValue(ValidationModeProperty, value);
        }
        public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.Register(nameof(ValidationMode), typeof(NumberBoxValidationMode),
            typeof(NumberBox), new PropertyMetadata(NumberBoxValidationMode.InvalidInputOverwritten));

        public INumberFormatter? NumberFormatter
        {
            get => (INumberFormatter?)GetValue(NumberFormatterProperty);
            set => SetValue(NumberFormatterProperty, value);
        }
        public static readonly DependencyProperty NumberFormatterProperty = DependencyProperty.Register(nameof(NumberFormatter), typeof(INumberFormatter),
            typeof(NumberBox), new PropertyMetadata(null, OnNumberFormatterChanged));


        public bool ClearButtonEnabled
        {
            get => (bool)GetValue(ClearButtonEnabledProperty);
            set => SetValue(ClearButtonEnabledProperty, value);
        }

        public static readonly DependencyProperty ClearButtonEnabledProperty =
            DependencyProperty.Register(nameof(ClearButtonEnabled), typeof(bool), typeof(NumberBox),
                new PropertyMetadata(true));
        #endregion

        /// <summary>
        /// Sự kiện này xảy ra sau khi người dùng kích hoạt quá trình đánh giá dữ liệu đầu vào mới bằng cách nhấn phím Enter, 
        /// nhấp vào nút xoay hoặc thay đổi tiêu điểm.
        /// </summary>
        public event RoutedEventHandler ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(nameof(ValueChanged), 
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(NumberBox));

        static NumberBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(typeof(NumberBox)));
            AcceptsReturnProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(false));
            MaxLinesProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(1));
            MinLinesProperty.OverrideMetadata(typeof(NumberBox), new FrameworkPropertyMetadata(1));
        }

        public NumberBox() : base()
        {

            HorizontalContentAlignment = HorizontalAlignment.Right;
            NumberFormatter ??= NumberBox.GetRegionalSettingsAwareDecimalFormatter();
            DataObject.AddPastingHandler(this, OnClipboardPaste);
            Loaded += (s, e) => UpdateTextToValue();
        }

        /// <summary> 
        /// Được gọi khi <see cref="Value"/> trong <see cref="NumberBox"/> thay đổi. 
        /// </summary>
        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not NumberBox numberBox) { return; }
            if (d is NumberBox nb && !nb._valueUpdating)
            {
                nb._valueUpdating = true;
                nb.UpdateTextToValue();
                nb._valueUpdating = false;
            }
            //numberBox.OnValueChanged(d, (double?)e.OldValue);
        }
        
        private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumberBox nb)
                nb.UpdateTextToValue();
        }

        private void UpdateTextToValue()
        {
            if (Value.HasValue)
            {
                string format = DecimalPlaces > 0 ? $"N{DecimalPlaces}" : "N0";
                string formatted = Value.Value.ToString(format, CultureInfo.CurrentCulture);
                //Text = Value.Value.ToString(format, CultureInfo.CurrentCulture);

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

        private void UpdateValueToText() 
        { 
            ValidateInput();
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                SetCurrentValue(ValueProperty, null);
                return;
            }

            if (double.TryParse(Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double result))
            {
                result = Math.Max(Minimum, Math.Min(Maximum, result));
                SetCurrentValue(ValueProperty, result);
            }
            else
            {
                UpdateTextToValue(); // Khôi phục giá trị cũ
            }

        }

        // Hỗ trợ Spin Button (nếu có trong template)
        public void Increment() => StepValue(SmallChange);
        public void Decrement() => StepValue(-SmallChange);
        public void LargeIncrement() => StepValue(LargeChange);
        public void LargeDecrement() => StepValue(-LargeChange);

        private void StepValue(double? change)
        {
            System.Diagnostics.Debug.WriteLine($"INFO: {typeof(NumberBox)} {nameof(StepValue)} raised, change {change}", "Libraries.Custom.Controls.Number.NumberBox");

            /// Before adjusting the value, validate the contents of the textbox so we don't override it.
            ValidateInput();

            var newValue = Value ?? 0;
            if (change is not null) { newValue += change ?? 0d; }

            SetCurrentValue(ValueProperty, newValue);
            MoveCaretToTextEnd();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (IsReadOnly) { return; }

            switch (e.Key)
            {
                case Key.PageUp:
                    LargeIncrement();
                    break;
                case Key.PageDown:
                    LargeDecrement();
                    break;
                case Key.Up:
                    Increment();
                    break;
                case Key.Down:
                    Decrement();
                    break;
                case Key.Enter:
                    if (TextWrapping != TextWrapping.Wrap)
                    {
                        ValidateInput();
                        MoveCaretToTextEnd();
                    }
                    break;
            }
        }

        protected override void OnTemplateButtonClick(string? parameter)
        {
            System.Diagnostics.Debug.WriteLine($"INFO: {typeof(NumberBox)} button clicked with param: {parameter}", "Libraries.Custom.Controls.Number.NumberBox");

            switch (parameter)
            {
                case "clear":
                    OnClearButtonClick();

                    break;
                case "increment":
                    Increment();
                    //StepValue(SmallChange);

                    break;
                case "decrement":
                    Decrement();
                    //StepValue(-SmallChange);

                    break;
            }

            /// NOTE: Focus looks and works well with mouse and Clear button. But it sucks for spin buttons
            _ = Focus();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateValueToText();// ValidateInput();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            // Cho phép số, dấu chấm, dấu phẩy, dấu trừ
            if (!"0123456789.,-".Contains(e.Text))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }


        protected override void OnTemplateChanged(System.Windows.Controls.ControlTemplate oldTemplate, System.Windows.Controls.ControlTemplate newTemplate)
        {
            base.OnTemplateChanged(oldTemplate, newTemplate);

            /// If Text has been set, but Value hasn't, update Value based on Text.
            if (string.IsNullOrEmpty(Text) && Value != null) { UpdateValueToText(); }
            else { UpdateTextToValue(); }
        }

        /// <summary> 
        /// Is called when something is pasted in this <see cref="NumberBox"/>. 
        /// </summary>
        protected virtual void OnClipboardPaste(object sender, DataObjectPastingEventArgs e)
        {
            /// TODO: Fix clipboard
            if (sender is not NumberBox) { return; }
            ValidateInput();
        }

        private void MoveCaretToTextEnd() 
        { 
            CaretIndex = Text.Length; 
        }

        private static INumberFormatter GetRegionalSettingsAwareDecimalFormatter() { return new ValidateNumberFormatter(); }

        private static void OnNumberFormatterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not INumberParser) { throw new InvalidOperationException($"{nameof(NumberFormatter)} must implement {typeof(INumberParser)}"); }
        }
    }
}
