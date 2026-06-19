using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// PasswordBox tùy chỉnh của Contellation, hỗ trợ Reveal Password, Placeholder, Icon, Clear Button.
    /// </summary>
    public partial class PasswordBox : TextBox
    {
        private readonly PasswordHelper _passwordHelper;
        private bool _isUpdating;

        #region Dependency Properties
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string),
                typeof(PasswordBox), new PropertyMetadata(string.Empty, OnPasswordChanged));

        public static readonly DependencyProperty PasswordCharProperty =
            DependencyProperty.Register(nameof(PasswordChar), typeof(char),
                typeof(PasswordBox), new PropertyMetadata('●', OnPasswordCharChanged));

        public static readonly DependencyProperty IsPasswordRevealedProperty =
            DependencyProperty.Register(nameof(IsPasswordRevealed), typeof(bool),
                typeof(PasswordBox), new PropertyMetadata(false, OnIsPasswordRevealedChanged));

        public static readonly DependencyProperty RevealButtonEnabledProperty =
            DependencyProperty.Register(nameof(RevealButtonEnabled), typeof(bool),
                typeof(PasswordBox), new PropertyMetadata(true));

        public static readonly RoutedEvent PasswordChangedEvent =
            EventManager.RegisterRoutedEvent(nameof(PasswordChanged), RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(PasswordBox));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public char PasswordChar
        {
            get => (char)GetValue(PasswordCharProperty);
            set => SetValue(PasswordCharProperty, value);
        }

        public bool IsPasswordRevealed
        {
            get => (bool)GetValue(IsPasswordRevealedProperty);
            private set => SetValue(IsPasswordRevealedProperty, value);
        }

        public bool RevealButtonEnabled
        {
            get => (bool)GetValue(RevealButtonEnabledProperty);
            set => SetValue(RevealButtonEnabledProperty, value);
        }

        public event RoutedEventHandler PasswordChanged
        {
            add => AddHandler(PasswordChangedEvent, value);
            remove => RemoveHandler(PasswordChangedEvent, value);
        }
        #endregion

        public static readonly RoutedCommand ToggleRevealCommand = new RoutedCommand("ToggleReveal", typeof(PasswordBox));

        public PasswordBox()
        {
            _passwordHelper = new PasswordHelper(this);
            CommandBindings.Add(new CommandBinding(ToggleRevealCommand, (s, e) => ToggleReveal()));
        }

        #region Event Handlers
        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb) pb.SyncTextWithPassword();
        }

        private static void OnPasswordCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb && !pb.IsPasswordRevealed)
                pb.UpdateMaskedText();
        }

        private static void OnIsPasswordRevealedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb) pb.SyncTextWithPassword();
        }
        #endregion

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_isUpdating)
            {
                base.OnTextChanged(e);
                return;
            }

            if (IsPasswordRevealed)
            {
                _isUpdating = true;
                SetCurrentValue(PasswordProperty, Text);
                _isUpdating = false;
            }
            else
            {
                _isUpdating = true;
                string newPassword = _passwordHelper.GetNewPassword(e.Changes);
                SetCurrentValue(PasswordProperty, newPassword);

                string masked = new string(PasswordChar, newPassword?.Length ?? 0);
                SetCurrentValue(TextProperty, masked);
                _isUpdating = false;
            }

            RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));
            SetPlaceholderTextVisibility();
            RevealClearButton();

            base.OnTextChanged(e);
        }
        private void SyncTextWithPassword()
        {
            _isUpdating = true;
            try
            {
                if (IsPasswordRevealed)
                {
                    SetCurrentValue(TextProperty, Password ?? string.Empty);
                }
                else
                {
                    SetCurrentValue(TextProperty, new string(PasswordChar, Password?.Length ?? 0));
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }
        private void UpdateMaskedText()
        {
            _isUpdating = true;
            SetCurrentValue(TextProperty, new string(PasswordChar, Password?.Length ?? 0));
            _isUpdating = false;
            //UpdateWithLock(() =>
            //{
            //    SetCurrentValue(TextProperty, new string(PasswordChar, Password?.Length ?? 0));
            //});
        }

        private void UpdateWithLock(Action action)
        {
            _isUpdating = true;
            try
            {
                action();
            }
            finally
            {
                _isUpdating = false;
            }
        }

        protected override void OnTemplateButtonClick(string? parameter)
        {
            if (parameter == "reveal")
            {
                SetCurrentValue(IsPasswordRevealedProperty, !IsPasswordRevealed);
                Focus();
                CaretIndex = Text.Length;
            }
            else
            {
                base.OnTemplateButtonClick(parameter);
            }
        }

        public void ToggleReveal()
        {
            SetCurrentValue(IsPasswordRevealedProperty, !IsPasswordRevealed);

            // Force update display
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SyncTextWithPassword();
                Focus();
                CaretIndex = Text.Length;
            }));
            //SetCurrentValue(IsPasswordRevealedProperty, !IsPasswordRevealed);
            //// Reset caret position
            //Dispatcher.BeginInvoke(new Action(() =>
            //{
            //    Focus();
            //    CaretIndex = Text.Length;
            //}));
        }

        private void SetPlaceholderTextVisibility() { /* Implement nếu cần */ }
        private void RevealClearButton() { /* Implement nếu cần */ }
    }
}
