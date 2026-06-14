using Contellation.Custom.Enums.Control.Placements;
using Contellation.Custom.Extensions.Inputs;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.TextBox"/> 
    /// with additional parameters like 
    /// <see cref="PlaceholderText"/>.
    /// </summary>
    public class TextBox : System.Windows.Controls.TextBox
    {
        /// <summary> 
        /// Gets or sets displayed <see cref="Icon"/>. 
        /// </summary>
        public string? Icon
        {
            get { return (string?)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon), typeof(string),
            typeof(TextBox), new PropertyMetadata(null));

        /// <summary> 
        /// Gets or sets FontFamily FontAwesomes for Icon <see cref="FontFamily"/>. 
        /// </summary>
        public FontFamily? FontFamilyIcon
        {
            get { return (FontFamily?)GetValue(FontFamilyIconProperty); }
            set { SetValue(FontFamilyIconProperty, value); }
        }
        public static readonly DependencyProperty FontFamilyIconProperty = DependencyProperty.Register(nameof(FontFamilyIcon), typeof(FontFamily),
            typeof(TextBox), new PropertyMetadata(null));

        /// <summary> 
        /// Gets or sets which side the icon should be placed on.
        /// </summary>
        public HorizontalPlacement IconPlacement
        {
            get { return (HorizontalPlacement)GetValue(IconPlacementProperty); }
            set { SetValue(IconPlacementProperty, value); }
        }
        public static readonly DependencyProperty IconPlacementProperty = DependencyProperty.Register(nameof(IconPlacement), typeof(HorizontalPlacement),
            typeof(TextBox), new PropertyMetadata(HorizontalPlacement.Left));

        /// <summary> 
        /// Gets or sets numbers pattern. 
        /// </summary>
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText), typeof(string),
            typeof(TextBox), new PropertyMetadata(string.Empty));

        /// <summary> 
        /// Gets or sets a value indicating whether to display the placeholder text. 
        /// </summary>
        public bool PlaceholderEnabled
        {
            get { return (bool)GetValue(PlaceholderEnabledProperty); }
            set { SetValue(PlaceholderEnabledProperty, value); }
        }
        public static readonly DependencyProperty PlaceholderEnabledProperty = DependencyProperty.Register(nameof(PlaceholderEnabled), typeof(bool),
            typeof(TextBox), new PropertyMetadata(true));

        /// <summary> 
        /// Gets or sets a value indicating whether to enable the clear button.
        /// </summary>
        public bool ClearButtonEnabled
        {
            get { return (bool)GetValue(ClearButtonEnabledProperty); }
            set { SetValue(ClearButtonEnabledProperty, value); }
        }
        public static readonly DependencyProperty ClearButtonEnabledProperty = DependencyProperty.Register(nameof(ClearButtonEnabled), typeof(bool),
            typeof(TextBox), new PropertyMetadata(true));

        /// <summary> 
        /// Gets or sets a value indicating whether to show the clear button when <see cref="TextBox"/> is focused. 
        /// </summary>
        public bool ShowClearButton
        {
            get { return (bool)GetValue(ShowClearButtonProperty); }
            protected set { SetValue(ShowClearButtonProperty, value); }
        }
        public static readonly DependencyProperty ShowClearButtonProperty = DependencyProperty.Register(nameof(ShowClearButton), typeof(bool),
            typeof(TextBox), new PropertyMetadata(false));

        /// <summary> 
        /// Gets or sets a value indicating whether text selection is enabled. 
        /// </summary>
        public bool IsTextSelectionEnabled
        {
            get { return (bool)GetValue(IsTextSelectionEnabledProperty); }
            set { SetValue(IsTextSelectionEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(nameof(IsTextSelectionEnabled), typeof(bool),
            typeof(TextBox), new PropertyMetadata(false));

        /// <summary> 
        /// Gets the command triggered when clicking the button. 
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(nameof(TemplateButtonCommand), typeof(IRelayCommand),
            typeof(TextBox), new PropertyMetadata(null));

        /// <summary> 
        /// Initializes a new instance of the <see cref="TextBox"/> class. 
        /// </summary>
        public TextBox()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<string>(OnTemplateButtonClick));
        }

        /// <inheritdoc />
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (PlaceholderEnabled && Text.Length > 0) { SetCurrentValue(PlaceholderEnabledProperty, false); }

            if (!PlaceholderEnabled && Text.Length < 1) { SetCurrentValue(PlaceholderEnabledProperty, true); }

            RevealClearButton();
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            CaretIndex = Text.Length;

            RevealClearButton();
        }

        /// <inheritdoc />
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            HideClearButton();
        }

        /// <summary> 
        /// Reveals the clear button by <see cref="ShowClearButton"/> property. 
        /// </summary>
        protected void RevealClearButton() { if (ClearButtonEnabled && IsKeyboardFocusWithin) { SetCurrentValue(ShowClearButtonProperty, Text.Length > 0); } }

        /// <summary> 
        /// Hides the clear button by <see cref="ShowClearButton"/> property. 
        /// </summary>
        protected void HideClearButton()
        {
            if (ClearButtonEnabled && !IsKeyboardFocusWithin && ShowClearButton) { SetCurrentValue(ShowClearButtonProperty, false); }
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
            Debug.WriteLine($"INFO: {typeof(TextBox)} button clicked", "Contellation.Custom.Controls.TextBox");

            OnClearButtonClick();
        }
    }
}
