using Contellation.Custom.Enums;
using Contellation.Custom.Extensions.Inputs;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// Extended <see cref="System.Windows.Controls.TextBox"/> 
    /// with additional parameters like 
    /// <see cref="PlaceholderText"/>.
    /// </summary>
    public class TextBox : System.Windows.Controls.TextBox
    {
        #region Dependency Properties

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

        #endregion

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

        #region Masked Input
        public string Mask
        {
            get => (string)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }
        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register(nameof(Mask), typeof(string),
                typeof(TextBox), new PropertyMetadata(string.Empty, OnMaskChanged));

        public char PromptChar
        {
            get => (char)GetValue(PromptCharProperty);
            set => SetValue(PromptCharProperty, value);
        }
        public static readonly DependencyProperty PromptCharProperty =
            DependencyProperty.Register(nameof(PromptChar), typeof(char),
                typeof(TextBox), new PropertyMetadata(' ', OnMaskChanged));

        public TextBoxMaskedFilterType Filter
        {
            get => (TextBoxMaskedFilterType)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(TextBoxMaskedFilterType),
                typeof(TextBox), new PropertyMetadata(TextBoxMaskedFilterType.Any, OnMaskChanged));

        private MaskedTextProvider? _maskProvider;
        private string _lastMask = string.Empty;

        #endregion

        /// <summary> 
        /// Initializes a new instance of the <see cref="TextBox"/> class. 
        /// </summary>
        public TextBox()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<string>(OnTemplateButtonClick));
        }

        private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox tb) tb.RefreshMaskProvider();
        }

        private void RefreshMaskProvider()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                _maskProvider = null;
                return;
            }

            _maskProvider = new MaskedTextProvider(Mask) { PromptChar = PromptChar };
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            UpdateStates();
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            CaretIndex = Text.Length;
            UpdateStates();
        }

        /// <inheritdoc />
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            UpdateStates();

            //HideClearButton();
        }

        private void UpdateStates()
        {
            bool shouldShowClear = ClearButtonEnabled && IsKeyboardFocusWithin && !string.IsNullOrEmpty(Text);
            SetValue(ShowClearButtonProperty, shouldShowClear);
            //SetValue(ShowClearButtonProperty, ClearButtonEnabled && IsKeyboardFocusWithin && !string.IsNullOrEmpty(Text));
        }


        /// <summary> 
        /// Triggered when the user clicks the clear text button. 
        /// </summary>
        protected virtual void OnClearButtonClick()
        {
            SetCurrentValue(TextProperty, string.Empty);
            Focus();
        }

        /// <summary> 
        /// Triggered by clicking a button in the control template. 
        /// </summary>
        protected virtual void OnTemplateButtonClick(string? parameter)
        {
            Debug.WriteLine($"INFO: {typeof(TextBox)} button clicked", "Contellation.Custom.Controls.TextBox");

            OnClearButtonClick();
        }

        // ==================== MASKED INPUT LOGIC ====================
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            if (IsReadOnly || string.IsNullOrEmpty(Mask) || _maskProvider == null)
            {
                base.OnPreviewTextInput(e);
                return;
            }

            int position = SelectionStart;
            bool success = _maskProvider.InsertAt(e.Text, position);

            if (success)
            {
                Text = _maskProvider.ToDisplayString();
                SelectionStart = GetSafeEditPosition(position + 1);
            }

            e.Handled = true;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(Mask) || _maskProvider == null)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            int position = SelectionStart;

            switch (e.Key)
            {
                case Key.Back when position > 0:
                    position--;
                    if (_maskProvider.RemoveAt(position))
                    {
                        Text = _maskProvider.ToDisplayString();
                        SelectionStart = GetSafeEditPosition(position);
                    }
                    e.Handled = true;
                    break;

                case Key.Delete when position < Text.Length:
                    if (_maskProvider.RemoveAt(position))
                    {
                        Text = _maskProvider.ToDisplayString();
                        SelectionStart = GetSafeEditPosition(position);
                    }
                    e.Handled = true;
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        private int GetSafeEditPosition(int startPosition)
        {
            if (_maskProvider == null)
                return Math.Max(0, startPosition);

            int pos = _maskProvider.FindEditPositionFrom(startPosition, true);
            return pos >= 0 ? pos : _maskProvider.FindEditPositionFrom(0, true);
        }
    }
}
