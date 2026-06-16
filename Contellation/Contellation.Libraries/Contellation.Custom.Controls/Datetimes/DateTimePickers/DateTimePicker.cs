using Contellation.Custom.Events;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Contellation.Custom.Controls
{
    /// <summary> 
    /// Công cụ chọn ngày và giờ
    /// </summary>
    [TemplatePart(Name = ElementRoot, Type = typeof(Grid))]
    [TemplatePart(Name = ElementTextBox, Type = typeof(TextBox))]
    [TemplatePart(Name = ElementButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementPopup, Type = typeof(Popup))]
    public class DateTimePicker : Control
    {
        private const string ElementRoot = "PART_Root";
        private const string ElementTextBox = "PART_TextBox";
        private const string ElementButton = "PART_Button";
        private const string ElementPopup = "PART_Popup";

        private CalendarWithClock _calendarWithClock;
        private TextBox _textBox;
        private Button _dropDownButton;
        private Popup _popup;

        private bool _disablePopupReopen;
        private DateTime? _originalSelectedDateTime;
        private readonly Dictionary<DependencyProperty, bool> _isHandlerSuspended = new();

        #region Routed Events

        public event EventHandler<FunctionEventArgs<DateTime?>> SelectedDateTimeChanged
        {
            add => AddHandler(SelectedDateTimeChangedEvent, value);
            remove => RemoveHandler(SelectedDateTimeChangedEvent, value);
        }
        public static readonly RoutedEvent SelectedDateTimeChangedEvent = EventManager.RegisterRoutedEvent(nameof(SelectedDateTimeChanged),
                RoutingStrategy.Direct, typeof(EventHandler<FunctionEventArgs<DateTime?>>), typeof(DateTimePicker));

        public event RoutedEventHandler PickerOpened;
        public event RoutedEventHandler PickerClosed;

        #endregion

        static DateTimePicker()
        {
            KeyboardNavigation.TabNavigationProperty.OverrideMetadata(typeof(DateTimePicker),
                new FrameworkPropertyMetadata(KeyboardNavigationMode.Once));

            KeyboardNavigation.IsTabStopProperty.OverrideMetadata(typeof(DateTimePicker),
                new FrameworkPropertyMetadata(false));
        }

        public DateTimePicker()
        {
            InitCalendarWithClock();
        }

        #region Dependency Properties

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text),
            typeof(string), typeof(DateTimePicker), new FrameworkPropertyMetadata(string.Empty, OnTextChanged));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker picker && !picker.IsHandlerSuspended(TextProperty) && picker._textBox != null)
            {
                picker._textBox.Text = e.NewValue as string ?? string.Empty;
            }
        }
        private bool IsHandlerSuspended(DependencyProperty property)
        {
            return _isHandlerSuspended.ContainsKey(property);
        }

        public string DateTimeFormat
        {
            get => (string)GetValue(DateTimeFormatProperty);
            set => SetValue(DateTimeFormatProperty, value);
        }
        public static readonly DependencyProperty DateTimeFormatProperty = DependencyProperty.Register(nameof(DateTimeFormat),
            typeof(string), typeof(DateTimePicker), new PropertyMetadata("dd/MM/yyyy HH:mm:ss", OnDateTimeFormatChanged));
        private static void OnDateTimeFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DateTimePicker picker && picker.SelectedDateTime.HasValue)
            {
                picker.SetTextInternal(picker.DateTimeToString(picker.SelectedDateTime.Value));
            }
        }

        public DateTime? SelectedDateTime
        {
            get => (DateTime?)GetValue(SelectedDateTimeProperty);
            set => SetValue(SelectedDateTimeProperty, value);
        }
        public static readonly DependencyProperty SelectedDateTimeProperty = DependencyProperty.Register(nameof(SelectedDateTime),
            typeof(DateTime?), typeof(DateTimePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnSelectedDateTimeChanged, CoerceSelectedDateTime));

        private static void OnSelectedDateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DateTimePicker picker) return;

            picker.SetTextInternal(picker.SelectedDateTime.HasValue
                ? picker.DateTimeToString(picker.SelectedDateTime.Value) : string.Empty);

            picker.RaiseEvent(new FunctionEventArgs<DateTime?>(SelectedDateTimeChangedEvent, picker)
            {
                Info = picker.SelectedDateTime
            });
        }
        private static object CoerceSelectedDateTime(DependencyObject d, object value)
        {
            if (d is DateTimePicker picker && picker._calendarWithClock != null)
            {
                picker._calendarWithClock.SelectedDateTime = (DateTime?)value;
                return picker._calendarWithClock.SelectedDateTime;
            }
            return value;
        }

        public DateTime DisplayDateTime
        {
            get => (DateTime)GetValue(DisplayDateTimeProperty);
            set => SetValue(DisplayDateTimeProperty, value);
        }
        public static readonly DependencyProperty DisplayDateTimeProperty = DependencyProperty.Register(nameof(DisplayDateTime),
                typeof(DateTime), typeof(DateTimePicker), new FrameworkPropertyMetadata(DateTime.Now,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceDisplayDateTime));

        private static object CoerceDisplayDateTime(DependencyObject d, object value)
        {
            if (d is DateTimePicker picker && picker._calendarWithClock != null)
            {
                picker._calendarWithClock.DisplayDateTime = (DateTime)value;
                return picker._calendarWithClock.DisplayDateTime;
            }
            return value;
        }

        public bool IsDropDownOpen
        {
            get => (bool)GetValue(IsDropDownOpenProperty);
            set => SetValue(IsDropDownOpenProperty, value);
        }
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(nameof(IsDropDownOpen),
            typeof(bool), typeof(DateTimePicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnIsDropDownOpenChanged, OnCoerceIsDropDownOpen));

        private static void OnIsDropDownOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DateTimePicker picker) return;
            if (picker._popup == null) return;

            bool isOpen = (bool)e.NewValue;
            if (picker._popup.IsOpen != isOpen)
            {
                picker._popup.IsOpen = isOpen;

                if (isOpen)
                {
                    picker._originalSelectedDateTime = picker.SelectedDateTime;
                    picker.Dispatcher.BeginInvoke(DispatcherPriority.Input, () => picker._calendarWithClock?.Focus());
                }
            }
        }
        private static object OnCoerceIsDropDownOpen(DependencyObject d, object baseValue)
            => d is DateTimePicker { IsEnabled: false } ? false : baseValue;

        public Style CalendarStyle
        {
            get => (Style)GetValue(CalendarStyleProperty);
            set => SetValue(CalendarStyleProperty, value);
        }
        public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register(nameof(CalendarStyle),
            typeof(Style), typeof(DateTimePicker), new PropertyMetadata(default(Style)));

        public Brush SelectionBrush
        {
            get => (Brush)GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }
        public static readonly DependencyProperty SelectionBrushProperty = TextBoxBase.SelectionBrushProperty.AddOwner(typeof(DateTimePicker));

        public Brush SelectionTextBrush
        {
            get => (Brush)GetValue(SelectionTextBrushProperty);
            set => SetValue(SelectionTextBrushProperty, value);
        }
        public static readonly DependencyProperty SelectionTextBrushProperty =
            TextBoxBase.SelectionTextBrushProperty.AddOwner(typeof(DateTimePicker));

        public double SelectionOpacity
        {
            get => (double)GetValue(SelectionOpacityProperty);
            set => SetValue(SelectionOpacityProperty, value);
        }
        public static readonly DependencyProperty SelectionOpacityProperty =
            TextBoxBase.SelectionOpacityProperty.AddOwner(typeof(DateTimePicker));

        public Brush CaretBrush
        {
            get => (Brush)GetValue(CaretBrushProperty);
            set => SetValue(CaretBrushProperty, value);
        }
        public static readonly DependencyProperty CaretBrushProperty =
            TextBoxBase.CaretBrushProperty.AddOwner(typeof(DateTimePicker));
        #endregion

        #region Coerce & Changed Handlers 
        #endregion

        #region Template

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Cleanup old template
            CleanupTemplate();

            _popup = GetTemplateChild(ElementPopup) as Popup;
            _dropDownButton = GetTemplateChild(ElementButton) as Button;
            _textBox = GetTemplateChild(ElementTextBox) as TextBox;

            if (_popup == null || _dropDownButton == null || _textBox == null)
                throw new Exception("Some template parts are missing.");

            SetupTemplateParts();
        }
        private void CleanupTemplate()
        {
            if (_popup != null)
            {
                _popup.PreviewMouseLeftButtonDown -= PopupPreviewMouseLeftButtonDown;
                _popup.Opened -= PopupOpened;
                _popup.Closed -= PopupClosed;
            }

            if (_dropDownButton != null)
            {
                _dropDownButton.Click -= DropDownButton_Click;
                _dropDownButton.MouseLeave -= DropDownButton_MouseLeave;
            }

            if (_textBox != null)
            {
                _textBox.KeyDown -= TextBox_KeyDown;
                _textBox.TextChanged -= TextBox_TextChanged;
                _textBox.LostFocus -= TextBox_LostFocus;
            }
        }
        private void SetupTemplateParts()
        {
            _popup.Child = _calendarWithClock;
            _popup.PreviewMouseLeftButtonDown += PopupPreviewMouseLeftButtonDown;
            _popup.Opened += PopupOpened;
            _popup.Closed += PopupClosed;

            _dropDownButton.Click += DropDownButton_Click;
            _dropDownButton.MouseLeave += DropDownButton_MouseLeave;

            SetupTextBox();
        }

        #endregion
        private void SetupTextBox()
        {
            _textBox.KeyDown += TextBox_KeyDown;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.LostFocus += TextBox_LostFocus;

            _textBox.SetBinding(TextBox.SelectionBrushProperty, new Binding(nameof(SelectionBrush)) { Source = this });
            _textBox.SetBinding(TextBox.SelectionTextBrushProperty, new Binding(nameof(SelectionTextBrush)) { Source = this });
            _textBox.SetBinding(TextBox.SelectionOpacityProperty, new Binding(nameof(SelectionOpacity)) { Source = this });
            _textBox.SetBinding(TextBox.CaretBrushProperty, new Binding(nameof(CaretBrush)) { Source = this });
        }

        private void InitCalendarWithClock()
        {
            _calendarWithClock = new CalendarWithClock { ShowConfirmButton = true };
            _calendarWithClock.SelectedDateTimeChanged += (_, e) => SelectedDateTime = e.Info;
            _calendarWithClock.Confirmed += () => IsDropDownOpen = false;
        }

        //private string DateTimeToString(DateTime dt) => dt.ToString(DateTimeFormat);
        private string DateTimeToString(DateTime dt)
        {
            try
            {
                return dt.ToString(DateTimeFormat, CultureInfo.CurrentCulture);
            }
            catch
            {
                return dt.ToString("dd/MM/yyyy HH:mm:ss"); // fallback
            }
        }

        private DateTime? ParseText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            // Ưu tiên parse theo DateTimeFormat
            // Thử parse với format hiện tại trước
            if (DateTime.TryParseExact(text, DateTimeFormat,
                CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }

            // Fallback parse thông thường
            if (DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out result))
                return result;

            return null;
        }

        private void SetTextInternal(string text)
        {
            SetCurrentValue(TextProperty, text);
        }

        // ==================== Các method Private ====================
        private void TextBox_LostFocus(object sender, RoutedEventArgs e) => SetSelectedDateTime();
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValueNoCallback(TextProperty, _textBox.Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = ProcessDateTimePickerKey(e) || e.Handled;
        }

        private bool ProcessDateTimePickerKey(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SetSelectedDateTime();
                return true;
            }

            if (e.Key == Key.System && e.SystemKey == Key.Down && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                TogglePopup();
                return true;
            }

            return false;
        }

        private void DropDownButton_Click(object sender, RoutedEventArgs e) => TogglePopup();
        private void DropDownButton_MouseLeave(object sender, MouseEventArgs e) => _disablePopupReopen = false;

        private void TogglePopup()
        {
            if (IsDropDownOpen)
            {
                SetCurrentValue(IsDropDownOpenProperty, false);
            }
            else
            {
                if (_disablePopupReopen)
                    _disablePopupReopen = false;
                else
                {
                    SetSelectedDateTime();
                    SetCurrentValue(IsDropDownOpenProperty, true);
                }
            }
        }

        private void SetSelectedDateTime()
        {
            if (_textBox == null) return;

            string currentText = _textBox.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(currentText))
            {
                SetCurrentValue(SelectedDateTimeProperty, null);
                return;
            }

            DateTime? parsed = ParseText(currentText);
            if (parsed.HasValue)
            {
                string formatted = DateTimeToString(parsed.Value);
                
                if (currentText != formatted)
                    SetTextInternal(formatted);

                if (SelectedDateTime != parsed.Value)
                {
                    SetCurrentValue(SelectedDateTimeProperty, parsed);
                    SetCurrentValue(DisplayDateTimeProperty, parsed);
                }
            }
            else
            {
                // Khôi phục giá trị cũ nếu parse thất bại
                if (SelectedDateTime.HasValue)
                    SetTextInternal(DateTimeToString(SelectedDateTime.Value));
                //SetTextInternal(SelectedDateTime.HasValue
                //    ? DateTimeToString(SelectedDateTime.Value)
                //    : DateTimeToString(DisplayDateTime));
            }


        }

        private void SetValueNoCallback(DependencyProperty property, object value)
        {
            _isHandlerSuspended[property] = true;
            try
            {
                SetCurrentValue(property, value);
            }
            finally
            {
                _isHandlerSuspended.Remove(property);
            }
        }

        private void PopupPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Popup { StaysOpen: false } &&
                _dropDownButton?.InputHitTest(e.GetPosition(_dropDownButton)) != null)
            {
                _disablePopupReopen = true;
            }
        }

        private void PopupOpened(object sender, EventArgs e)
        {
            if (!IsDropDownOpen) SetCurrentValue(IsDropDownOpenProperty, true);
            _calendarWithClock?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            OnPickerOpened(new RoutedEventArgs());
        }

        private void PopupClosed(object sender, EventArgs e)
        {
            if (IsDropDownOpen) SetCurrentValue(IsDropDownOpenProperty, false);
            if (_calendarWithClock?.IsKeyboardFocusWithin == true)
                MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            OnPickerClosed(new RoutedEventArgs());
        }

        protected virtual void OnPickerOpened(RoutedEventArgs e) => PickerOpened?.Invoke(this, e);
        protected virtual void OnPickerClosed(RoutedEventArgs e) => PickerClosed?.Invoke(this, e);

        public override string ToString() => SelectedDateTime?.ToString(DateTimeFormat) ?? string.Empty;

    }
}
