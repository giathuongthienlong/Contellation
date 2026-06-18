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
            //CommandBindings.Add(new CommandBinding(ToggleRevealCommand, (s, e) => ToggleReveal()));
            _passwordHelper = new PasswordHelper(this);
        }

        #region Event Handlers

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb) pb.UpdateTextContents();
        }

        private static void OnPasswordCharChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb && !pb.IsPasswordRevealed)
                pb.UpdateMaskedText();
        }

        private static void OnIsPasswordRevealedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox pb) pb.UpdateTextContents();
        }

        #endregion

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            if (_isUpdating)
            {
                base.OnTextChanged(e);
                return;
            }
            // Chỉ xử lý khi người dùng đang gõ (không lock)
            if (IsPasswordRevealed)
            {
                UpdateWithLock(() =>
                {
                    SetCurrentValue(PasswordProperty, Text);
                    RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));
                });
            }
            else
            {
                // Chế độ ẩn mật khẩu
                UpdateWithLock(() =>
                {
                    // Cập nhật Password từ input
                    string newPassword = _passwordHelper.GetNewPassword(e.Changes);
                    SetCurrentValue(PasswordProperty, newPassword);

                    // Hiển thị mask
                    string masked = new string(PasswordChar, newPassword?.Length ?? 0);
                    SetCurrentValue(TextProperty, masked);

                    RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));
                });
            }

            SetPlaceholderTextVisibility();
            RevealClearButton();

            base.OnTextChanged(e);
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

        private void UpdateTextContents()
        {
            if (IsPasswordRevealed)
            {
                HandleRevealedMode();
            }
            else
            {
                HandleHiddenMode();
            }
        }

        private void HandleRevealedMode()
        {
            if (Password != Text)
            {
                UpdateWithLock(() =>
                {
                    SetCurrentValue(PasswordProperty, Text);
                    RaiseEvent(new RoutedEventArgs(PasswordChangedEvent));
                });
            }
        }

        private void HandleHiddenMode()
        {
            UpdateWithLock(() =>
            {
                string maskedText = new string(PasswordChar, Password?.Length ?? 0);
                SetCurrentValue(TextProperty, maskedText);
            });
        }

        private void UpdateMaskedText()
        {
            UpdateWithLock(() =>
            {
                SetCurrentValue(TextProperty, new string(PasswordChar, Password?.Length ?? 0));
            });
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
            //action();
            _isUpdating = false;
        }



        private void SetPlaceholderTextVisibility()
        {
            // Sử dụng Attached Property nếu có
        }

        private void RevealClearButton()
        {
            // Logic ClearButton nếu cần
        }
    }
}
