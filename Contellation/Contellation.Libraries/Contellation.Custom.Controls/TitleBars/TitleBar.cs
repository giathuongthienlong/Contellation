using Contellation.Custom.Enums.Control;
using Contellation.Custom.Events;
using Contellation.Custom.Extensions;
using Contellation.Custom.Extensions.Inputs;
using Contellation.Custom.Helpers;
using Contellation.Custom.Interops;
using Contellation.Custom.Structs;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static Contellation.Custom.Interops.User32;

namespace Contellation.Custom.Controls
{
    [TemplatePart(Name = ElementMainGrid, Type = typeof(System.Windows.Controls.Grid))]
    [TemplatePart(Name = ElementIcon, Type = typeof(System.Windows.Controls.Image))]
    [TemplatePart(Name = ElementHelpButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementMinimizeButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementMaximizeButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementRestoreButton, Type = typeof(Button))]
    [TemplatePart(Name = ElementCloseButton, Type = typeof(Button))]
    public partial class TitleBar : System.Windows.Controls.Control
    {
        private const string ElementIcon = "PART_Icon";
        private const string ElementMainGrid = "PART_MainGrid";
        private const string ElementHelpButton = "PART_HelpButton";
        private const string ElementMinimizeButton = "PART_MinimizeButton";
        private const string ElementMaximizeButton = "PART_MaximizeButton";
        private const string ElementRestoreButton = "PART_RestoreButton";
        private const string ElementCloseButton = "PART_CloseButton";

        private static DpiScale? dpiScale;

        private readonly Thickness _commonPadding;
        private double _tempNonClientAreaHeight;
        private DependencyObject? _parentWindow;

        public event EventHandler<HwndProcEventArgs>? WndProcInvoked;


        /// <summary>Identifies the <see cref="Title"/> dependency property.</summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title),
            typeof(string),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

        /// <summary>Identifies the <see cref="Header"/> dependency property.</summary>
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            nameof(Header),
            typeof(object),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

        /// <summary>
        /// Property for <see cref="CenterContent"/>.
        /// </summary>
        public static readonly DependencyProperty CenterContentProperty = DependencyProperty.Register(
            nameof(CenterContent),
            typeof(object),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

        /// <summary>
        /// Property for <see cref="TrailingContent"/>.
        /// </summary>
        public static readonly DependencyProperty TrailingContentProperty = DependencyProperty.Register(
            nameof(TrailingContent),
            typeof(object),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

        /// <summary>Identifies the <see cref="ButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
            nameof(ButtonsForeground),
            typeof(Brush),
            typeof(TitleBar),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits
            )
        );

        /// <summary>Identifies the <see cref="ButtonsBackground"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonsBackgroundProperty = DependencyProperty.Register(
            nameof(ButtonsBackground),
            typeof(Brush),
            typeof(TitleBar),
            new FrameworkPropertyMetadata(SystemColors.ControlBrush, FrameworkPropertyMetadataOptions.Inherits)
        );

        /// <summary>Identifies the <see cref="IsMaximized"/> dependency property.</summary>
        public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(
            nameof(IsMaximized),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
        );

        /// <summary>Identifies the <see cref="ForceShutdown"/> dependency property.</summary>
        public static readonly DependencyProperty ForceShutdownProperty = DependencyProperty.Register(
            nameof(ForceShutdown),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
        );

        /// <summary>Identifies the <see cref="ShowMaximize"/> dependency property.</summary>
        public static readonly DependencyProperty ShowMaximizeProperty = DependencyProperty.Register(
            nameof(ShowMaximize),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
        );

        /// <summary>Identifies the <see cref="ShowMinimize"/> dependency property.</summary>
        public static readonly DependencyProperty ShowMinimizeProperty = DependencyProperty.Register(
            nameof(ShowMinimize),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
        );

        /// <summary>Identifies the <see cref="ShowHelp"/> dependency property.</summary>
        public static readonly DependencyProperty ShowHelpProperty = DependencyProperty.Register(
            nameof(ShowHelp),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(false)
        );

        /// <summary>Identifies the <see cref="ShowClose"/> dependency property.</summary>
        public static readonly DependencyProperty ShowCloseProperty = DependencyProperty.Register(
            nameof(ShowClose),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
        );

        /// <summary>Identifies the <see cref="CanMaximize"/> dependency property.</summary>
        public static readonly DependencyProperty CanMaximizeProperty = DependencyProperty.Register(
            nameof(CanMaximize),
            typeof(bool),
            typeof(TitleBar),
            new PropertyMetadata(true)
        );

        /// <summary>Identifies the <see cref="Icon"/> dependency property.</summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon),
            typeof(string),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );

        /// <summary>Identifies the <see cref="CloseWindowByDoubleClickOnIcon"/> dependency property.</summary>
        public static readonly DependencyProperty CloseWindowByDoubleClickOnIconProperty =
            DependencyProperty.Register(
                nameof(CloseWindowByDoubleClickOnIcon),
                typeof(bool),
                typeof(TitleBar),
                new PropertyMetadata(false)
            );

        /// <summary>Identifies the <see cref="CloseClicked"/> routed event.</summary>
        public static readonly RoutedEvent CloseClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(CloseClicked),
            RoutingStrategy.Bubble,
            typeof(TypedEventHandler<TitleBar, RoutedEventArgs>),
            typeof(TitleBar)
        );

        /// <summary>Identifies the <see cref="MaximizeClicked"/> routed event.</summary>
        public static readonly RoutedEvent MaximizeClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(MaximizeClicked),
            RoutingStrategy.Bubble,
            typeof(TypedEventHandler<TitleBar, RoutedEventArgs>),
            typeof(TitleBar)
        );

        /// <summary>Identifies the <see cref="MinimizeClicked"/> routed event.</summary>
        public static readonly RoutedEvent MinimizeClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(MinimizeClicked),
            RoutingStrategy.Bubble,
            typeof(TypedEventHandler<TitleBar, RoutedEventArgs>),
            typeof(TitleBar)
        );

        /// <summary>Identifies the <see cref="HelpClicked"/> routed event.</summary>
        public static readonly RoutedEvent HelpClickedEvent = EventManager.RegisterRoutedEvent(
            nameof(HelpClicked),
            RoutingStrategy.Bubble,
            typeof(TypedEventHandler<TitleBar, RoutedEventArgs>),
            typeof(TitleBar)
        );

        /// <summary>Identifies the <see cref="TemplateButtonCommand"/> dependency property.</summary>
        public static readonly DependencyProperty TemplateButtonCommandProperty = DependencyProperty.Register(
            nameof(TemplateButtonCommand),
            typeof(IRelayCommand),
            typeof(TitleBar),
            new PropertyMetadata(null)
        );


        /// <summary>
        /// Gets or sets title displayed on the left.
        /// </summary>
        public string? Title
        {
            get => (string?)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed in the center of the <see cref="TitleBar"/>.
        /// </summary>
        public object? CenterContent
        {
            get => GetValue(CenterContentProperty);
            set => SetValue(CenterContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed in right side of the <see cref="TitleBar"/>.
        /// </summary>
        public object? TrailingContent
        {
            get => GetValue(TrailingContentProperty);
            set => SetValue(TrailingContentProperty, value);
        }

        /// <summary>
        /// Gets or sets the content displayed in the <see cref="TitleBar"/>.
        /// </summary>
        public object? Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }

        /// <summary>
        /// Gets or sets the foreground of the navigation buttons.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush ButtonsForeground
        {
            get => (Brush)GetValue(ButtonsForegroundProperty);
            set => SetValue(ButtonsForegroundProperty, value);
        }

        /// <summary>
        /// Gets or sets the background of the navigation buttons when hovered.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush ButtonsBackground
        {
            get => (Brush)GetValue(ButtonsBackgroundProperty);
            set => SetValue(ButtonsBackgroundProperty, value);
        }

        /// <summary>
        /// Gets a value indicating whether the current window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get => (bool)GetValue(IsMaximizedProperty);
            internal set => SetValue(IsMaximizedProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the controls affect main application window.
        /// </summary>
        public bool ForceShutdown
        {
            get => (bool)GetValue(ForceShutdownProperty);
            set => SetValue(ForceShutdownProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the maximize button.
        /// </summary>
        public bool ShowMaximize
        {
            get => (bool)GetValue(ShowMaximizeProperty);
            set => SetValue(ShowMaximizeProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the minimize button.
        /// </summary>
        public bool ShowMinimize
        {
            get => (bool)GetValue(ShowMinimizeProperty);
            set => SetValue(ShowMinimizeProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the help button
        /// </summary>
        public bool ShowHelp
        {
            get => (bool)GetValue(ShowHelpProperty);
            set => SetValue(ShowHelpProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the close button.
        /// </summary>
        public bool ShowClose
        {
            get => (bool)GetValue(ShowCloseProperty);
            set => SetValue(ShowCloseProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the maximize functionality is enabled. If disabled the MaximizeActionOverride action won't be called
        /// </summary>
        public bool CanMaximize
        {
            get => (bool)GetValue(CanMaximizeProperty);
            set => SetValue(CanMaximizeProperty, value);
        }

        /// <summary>
        /// Gets or sets the titlebar icon.
        /// </summary>
        public string? Icon
        {
            get => (string?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window can be closed by double clicking on the icon
        /// </summary>
        public bool CloseWindowByDoubleClickOnIcon
        {
            get => (bool)GetValue(CloseWindowByDoubleClickOnIconProperty);
            set => SetValue(CloseWindowByDoubleClickOnIconProperty, value);
        }

        /// <summary>
        /// Event triggered after clicking close button.
        /// </summary>
        public event TypedEventHandler<TitleBar, RoutedEventArgs> CloseClicked
        {
            add => AddHandler(CloseClickedEvent, value);
            remove => RemoveHandler(CloseClickedEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking maximize or restore button.
        /// </summary>
        public event TypedEventHandler<TitleBar, RoutedEventArgs> MaximizeClicked
        {
            add => AddHandler(MaximizeClickedEvent, value);
            remove => RemoveHandler(MaximizeClickedEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking minimize button.
        /// </summary>
        public event TypedEventHandler<TitleBar, RoutedEventArgs> MinimizeClicked
        {
            add => AddHandler(MinimizeClickedEvent, value);
            remove => RemoveHandler(MinimizeClickedEvent, value);
        }

        /// <summary>
        /// Event triggered after clicking help button
        /// </summary>
        public event TypedEventHandler<TitleBar, RoutedEventArgs> HelpClicked
        {
            add => AddHandler(HelpClickedEvent, value);
            remove => RemoveHandler(HelpClickedEvent, value);
        }

        /// <summary>
        /// Gets the command triggered when clicking the titlebar button.
        /// </summary>
        public IRelayCommand TemplateButtonCommand => (IRelayCommand)GetValue(TemplateButtonCommandProperty);

        /// <summary>
        /// Gets or sets the <see cref="Action"/> that should be executed when the Maximize button is clicked."/>
        /// </summary>
        public Action<TitleBar, System.Windows.Window>? MaximizeActionOverride { get; set; }

        /// <summary>
        /// Gets or sets what <see cref="Action"/> should be executed when the Minimize button is clicked.
        /// </summary>
        public Action<TitleBar, System.Windows.Window>? MinimizeActionOverride { get; set; }

        //private readonly Button[] _buttons = new Button[4];
        private readonly List<Button> _buttons = new List<Button>();
        private readonly TextBlock _titleBlock;
        private System.Windows.Window _currentWindow = null!;

        /*private System.Windows.Controls.Grid _mainGrid = null!;*/
        private System.Windows.Controls.ContentPresenter _icon = null!;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleBar"/> class and sets the default <see cref="FrameworkElement.Loaded"/> event.
        /// </summary>
        public TitleBar()
        {
            SetValue(TemplateButtonCommandProperty, new RelayCommand<ButtonAction>(OnTemplateButtonClick));

            dpiScale ??= VisualTreeHelper.GetDpi(this);

            _titleBlock = new TextBlock();
            _titleBlock.VerticalAlignment = VerticalAlignment.Center;
            _ = _titleBlock.SetBinding(
                System.Windows.Controls.TextBlock.TextProperty,
                new Binding(nameof(Title)) { Source = this }
            );
            _ = _titleBlock.SetBinding(
                System.Windows.Controls.TextBlock.FontSizeProperty,
                new Binding(nameof(FontSize)) { Source = this }
            );
            _ = _titleBlock.SetBinding(
                System.Windows.Controls.TextBlock.FontWeightProperty,
                new Binding(nameof(FontWeight)) { Source = this }
            );
            Header = _titleBlock;


            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        /// <inheritdoc />
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        protected virtual void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DesignerHelper.IsInDesignMode) { return; }

            _currentWindow =
                System.Windows.Window.GetWindow(this) ?? throw new InvalidOperationException("Window is null");

            if (_currentWindow.WindowState == WindowState.Maximized)
            {
                SetCurrentValue(IsMaximizedProperty, true);
                _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Maximized);
            }

            _currentWindow.StateChanged += OnParentWindowStateChanged;
            _currentWindow.ContentRendered += OnWindowContentRendered;

        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            Unloaded -= OnUnloaded;
        }

        /// <summary>
        /// Invoked whenever application code or an internal process,
        /// such as a rebuilding layout pass, calls the ApplyTemplate method.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _parentWindow = VisualTreeHelper.GetParent(this);

            while (_parentWindow is not null and not Window)
            {
                _parentWindow = VisualTreeHelper.GetParent(_parentWindow);
            }

            MouseRightButtonUp += TitleBar_MouseRightButtonUp;

            /*_mainGrid = GetTemplateChild<System.Windows.Controls.Grid>(ElementMainGrid);*/
            _icon = GetTemplateChild<System.Windows.Controls.ContentPresenter>(ElementIcon);

            Button helpButton = GetTemplateChild<Button>(ElementHelpButton);
            Button minimizeButton = GetTemplateChild<Button>(ElementMinimizeButton);
            Button maximizeButton = GetTemplateChild<Button>(ElementMaximizeButton);
            Button closeButton = GetTemplateChild<Button>(ElementCloseButton);

            _buttons.Add(maximizeButton);
            _buttons.Add(minimizeButton);
            _buttons.Add(closeButton);
            _buttons.Add(helpButton);
        }


        private void CloseWindow()
        {
            Debug.WriteLine(
                $"INFO | {typeof(TitleBar)}.CloseWindow:ForceShutdown -  {ForceShutdown}",
                "Contellation.Custom.Controls.TitleBar"
            );

            if (ForceShutdown)
            {
                Application.Current.Shutdown();
                return;
            }

            _currentWindow.Close();
        }

        private void MinimizeWindow()
        {
            if (MinimizeActionOverride is not null)
            {
                MinimizeActionOverride(this, _currentWindow);

                return;
            }

            _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Minimized);
        }

        private void MaximizeWindow()
        {
            if (!CanMaximize) { return; }

            if (MaximizeActionOverride is not null)
            {
                MaximizeActionOverride(this, _currentWindow);

                return;
            }

            if (_currentWindow.WindowState == WindowState.Normal)
            {
                SetCurrentValue(IsMaximizedProperty, true);
                _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Maximized);
            }
            else
            {
                SetCurrentValue(IsMaximizedProperty, false);
                _currentWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
            }
        }

        private void OnParentWindowStateChanged(object? sender, EventArgs e)
        {
            if (IsMaximized != (_currentWindow.WindowState == WindowState.Maximized))
            {
                SetCurrentValue(IsMaximizedProperty, _currentWindow.WindowState == WindowState.Maximized);
            }
        }

        private void OnTemplateButtonClick(ButtonAction buttonType)
        {
            switch (buttonType)
            {
                case ButtonAction.Maximize
                or ButtonAction.Restore:
                    RaiseEvent(new RoutedEventArgs(MaximizeClickedEvent, this));
                    MaximizeWindow();
                    break;

                case ButtonAction.Close:
                    RaiseEvent(new RoutedEventArgs(CloseClickedEvent, this));
                    CloseWindow();
                    break;

                case ButtonAction.Minimize:
                    RaiseEvent(new RoutedEventArgs(MinimizeClickedEvent, this));
                    MinimizeWindow();
                    break;

                case ButtonAction.Help:
                    RaiseEvent(new RoutedEventArgs(HelpClickedEvent, this));
                    break;
            }
        }

        /// <summary>
        ///     Listening window hooks after rendering window content to SizeToContent support
        /// </summary>
        private void OnWindowContentRendered(object? sender, EventArgs e)
        {
            if (sender is not Window window) { return; }

            window.ContentRendered -= OnWindowContentRendered;

            IntPtr handle = new WindowInteropHelper(window).Handle;
            HwndSource windowSource =
                HwndSource.FromHwnd(handle) ?? throw new InvalidOperationException("Window source is null");
            windowSource.AddHook(HwndSourceHook);
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var message = (User32.WM)msg;

            if (message is not (User32.WM.NCHITTEST or User32.WM.NCMOUSELEAVE or User32.WM.NCLBUTTONDOWN or User32.WM.NCLBUTTONUP))
            {
                return IntPtr.Zero;
            }

            foreach (Button button in _buttons)
            {
                if (button is null || !button.ReactToHwndHook(message, lParam, out IntPtr returnIntPtr))
                {
                    continue;
                }

                // Fix for when sometimes, button hover backgrounds aren't cleared correctly, causing multiple buttons to appear as if hovered.
                foreach (Button anotherButton in _buttons)
                {
                    if (anotherButton is null || anotherButton == button) { continue; }

                    if (anotherButton.IsHovered && button.IsHovered)
                    {
                        anotherButton.RemoveHover();
                    }
                }

                handled = true;
                return returnIntPtr;
            }

            bool isMouseOverHeaderContent = false;
            IntPtr htResult = (IntPtr)WM_NCHITTEST.HTNOWHERE;

            if (message == User32.WM.NCHITTEST && Header is UIElement headerUiElement)
            {
                isMouseOverHeaderContent = headerUiElement.IsMouseOverElement(lParam);
            }

            var e = new HwndProcEventArgs(hwnd, msg, wParam, lParam, isMouseOverHeaderContent);
            WndProcInvoked?.Invoke(this, e);

            if (e.ReturnValue != null)
            {
                handled = e.Handled;
                return e.ReturnValue ?? IntPtr.Zero;
            }

            switch (message)
            {
                case User32.WM.NCHITTEST when CloseWindowByDoubleClickOnIcon && _icon.IsMouseOverElement(lParam):
                    // Ideally, clicking on the icon should open the system menu, but when the system menu is opened manually, double-clicking on the icon does not close the window
                    handled = true;
                    return (IntPtr)User32.WM_NCHITTEST.HTSYSMENU;
                case User32.WM.NCHITTEST when htResult != (IntPtr)WM_NCHITTEST.HTNOWHERE:
                    handled = true;
                    return htResult;
                case User32.WM.NCHITTEST when this.IsMouseOverElement(lParam) && !isMouseOverHeaderContent:
                    handled = true;
                    return (IntPtr)User32.WM_NCHITTEST.HTCAPTION;
                default:
                    return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Show 'SystemMenu' on mouse right button up.
        /// </summary>
        private void TitleBar_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Point point = PointToScreen(e.GetPosition(this));

            if (dpiScale is null)
            {
                throw new InvalidOperationException("dpiScale is not initialized.");
            }

            SystemCommands.ShowSystemMenu(
                _parentWindow as Window,
                new Point(point.X / dpiScale.Value.DpiScaleX, point.Y / dpiScale.Value.DpiScaleY)
            );
        }

        private void WmGetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
            var monitor = InteropMethods.MonitorFromWindow(hwnd, 0x00000002);

            if (monitor != IntPtr.Zero && mmi != null)
            {
                APPBARDATA appBarData = default;
                var autoHide = InteropMethods.SHAppBarMessage(4, ref appBarData) != 0;
                if (autoHide)
                {
                    var monitorInfo = default(MONITORINFO);
                    monitorInfo.cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFO));
                    InteropMethods.GetMonitorInfo(monitor, ref monitorInfo);
                    var rcWorkArea = monitorInfo.rcWork;
                    var rcMonitorArea = monitorInfo.rcMonitor;
                    mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.Left - rcMonitorArea.Left);
                    mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.Top - rcMonitorArea.Top);
                    mmi.ptMaxSize.x = Math.Abs(rcWorkArea.Right - rcWorkArea.Left);
                    mmi.ptMaxSize.y = Math.Abs(rcWorkArea.Bottom - rcWorkArea.Top - 1);
                }
            }

            Marshal.StructureToPtr(mmi, lParam, true);
        }
        private T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            DependencyObject element = GetTemplateChild(name);

            if (element is not T tElement)
            {
                throw new InvalidOperationException($"Template part '{name}' is not found or is not of type {typeof(T)}");
            }

            return tElement;
        }
    }
}
