using Contellation.Custom.Extensions.Inputs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Contellation.Custom.Controls
{
    [TemplatePart(Name = ElementTextBox, Type = typeof(DatePickerTextBox))]
    public class DatePicker : System.Windows.Controls.DatePicker
    {
        private const string ElementTextBox = "PART_TextBox";

        private TextBox _textBox;

        /// <summary> 
        /// Gets the command triggered when clicking the button. 
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(DatePicker), new PropertyMetadata(null));

        /// <summary> 
        /// Gets or sets numbers pattern. 
        /// </summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText), typeof(string),
            typeof(DatePicker), new PropertyMetadata(string.Empty));


        /// <summary> 
        /// Gets or sets a value indicating whether text selection is enabled. 
        /// </summary>
        public bool IsTextSelectionEnabled
        {
            get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
            set { SetValue(IsTextSelectionEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(nameof(IsTextSelectionEnabled), typeof(bool),
            typeof(DatePicker), new PropertyMetadata(false));

        /// <summary> 
        /// Initializes a new instance of the <see cref="DatePicker"/> class. 
        /// </summary>
        public DatePicker()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<string>(OnTemplateButtonClick));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _textBox = GetTemplateChild(ElementTextBox) as TextBox;
        }

        /// <summary> 
        /// Triggered when the user clicks the clear text button. 
        /// </summary>
        protected virtual void OnClearButtonClick() { if (Text.Length > 0) { SetCurrentValue(TextProperty, string.Empty); } }

        /// <summary> 
        /// Triggered by clicking a button in the control template.
        /// </summary>
        protected virtual void OnTemplateButtonClick(string? parameter)
        {
            Debug.WriteLine($"INFO: {typeof(DatePicker)} button clicked", "Contellation.Custom.Controls.DatePicker");

            OnClearButtonClick();
        }
    }
}
