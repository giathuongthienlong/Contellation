using Contellation.Custom.Controls.Windows;
using Contellation.Custom.Enums.Control;
using Contellation.Custom.Helpers;
using Contellation.Custom.Interops;
using Contellation.Custom.Interops.Unsafes;
using Contellation.Custom.Structs;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shell;

namespace Contellation.Custom.Controls
{
    public class Window : System.Windows.Window
    {
        private double _tempNonClientAreaHeight;
        private readonly Thickness _commonPadding;

        private WindowInteropHelper? _interopHelper = null;
        /// <summary>
        /// Gets contains helper for accessing this window handle.
        /// </summary>
        protected WindowInteropHelper InteropHelper
        {
            get => _interopHelper ??= new WindowInteropHelper(this);
        }

        /// <summary>
        /// Gets or sets a value determining corner preference for current <see cref="Window"/>.
        /// </summary>
        public WindowCornerPreference WindowCornerPreference
        {
            get => (WindowCornerPreference)GetValue(WindowCornerPreferenceProperty);
            set => SetValue(WindowCornerPreferenceProperty, value);
        }
        /// <summary>Identifies the <see cref="WindowCornerPreference"/> dependency property.</summary>
        public static readonly DependencyProperty WindowCornerPreferenceProperty = DependencyProperty.Register(
            nameof(WindowCornerPreference),
            typeof(WindowCornerPreference),
            typeof(Window),
            new PropertyMetadata(WindowCornerPreference.Round, OnWindowCornerPreferenceChanged)
        );

        /// <summary>
        /// Gets or sets a value determining preferred backdrop type for current <see cref="Window"/>.
        /// </summary>
        public WindowBackdropType WindowBackdropType
        {
            get => (WindowBackdropType)GetValue(WindowBackdropTypeProperty);
            set => SetValue(WindowBackdropTypeProperty, value);
        }
        /// <summary>Identifies the <see cref="WindowBackdropType"/> dependency property.</summary>
        public static readonly DependencyProperty WindowBackdropTypeProperty = DependencyProperty.Register(
            nameof(WindowBackdropType),
            typeof(WindowBackdropType),
            typeof(Window),
            new PropertyMetadata(WindowBackdropType.None, OnWindowBackdropTypeChanged)
        );

        /// <summary>
        /// Gets or sets a value indicating whether the default title bar of the window should be hidden to create space for app content.
        /// </summary>
        public bool ExtendsContentIntoTitleBar
        {
            get => (bool)GetValue(ExtendsContentIntoTitleBarProperty);
            set => SetValue(ExtendsContentIntoTitleBarProperty, value);
        }
        /// <summary>Identifies the <see cref="ExtendsContentIntoTitleBar"/> dependency property.</summary>
        public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty =
            DependencyProperty.Register(
                nameof(ExtendsContentIntoTitleBar),
                typeof(bool),
                typeof(Window),
                new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged)
            );

        /// <summary>
        /// Area Bar Height
        /// </summary>
        public double AreaBarHeight
        {
            get { return (double)GetValue(AreaBarHeightProperty); }
            set { SetValue(AreaBarHeightProperty, value); }
        }
        public static readonly DependencyProperty AreaBarHeightProperty = DependencyProperty.Register(nameof(AreaBarHeight), typeof(double),
            typeof(Window), new PropertyMetadata(22.0));

        /// <summary>
        /// Gets a value indicating whether the current window is maximized.
        /// </summary>
        public bool IsMaximized
        {
            get { return (bool)GetValue(IsMaximizedProperty); }
            internal set { SetValue(IsMaximizedProperty, value); }
        }
        public static readonly DependencyProperty IsMaximizedProperty = DependencyProperty.Register(nameof(IsMaximized), typeof(bool),
            typeof(Window), new PropertyMetadata(false));

        static Window()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(typeof(Window)));
            //StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(ResourceHelper.GetResourceInternal<Style>("WindowCustom")));
        }

        public Window()
        {
            SetResourceReference(StyleProperty, typeof(Window));

            _commonPadding = Padding;

            Loaded += (s, e) => OnLoaded(e);
        }

        protected void OnLoaded(RoutedEventArgs args)
        {
            ContentRendered += OnWindowContentRendered;
            if (SizeToContent != SizeToContent.WidthAndHeight) { return; }

            SizeToContent = SizeToContent.Height;
            Dispatcher.BeginInvoke(new Action(() => { SizeToContent = SizeToContent.WidthAndHeight; }));
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState == WindowState.Maximized)
            {
                BorderThickness = new Thickness();
                _tempNonClientAreaHeight = AreaBarHeight;
                SetCurrentValue(IsMaximizedProperty, true);
                AreaBarHeight += 8;
            }
            else
            {
                SetCurrentValue(IsMaximizedProperty, false);
                AreaBarHeight = _tempNonClientAreaHeight;
            }
        }

        /// <inheritdoc />
        protected override void OnSourceInitialized(EventArgs e)
        {
            OnCornerPreferenceChanged(default, WindowCornerPreference);
            OnExtendsContentIntoTitleBarChanged(default, ExtendsContentIntoTitleBar);
            OnBackdropTypeChanged(default, WindowBackdropType);

            base.OnSourceInitialized(e);
        }

        /// <summary>
        /// Private <see cref="WindowCornerPreference"/> property callback.
        /// </summary>
        private static void OnWindowCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window)
            {
                return;
            }

            if (e.OldValue == e.NewValue)
            {
                return;
            }

            window.OnCornerPreferenceChanged(
                (WindowCornerPreference)e.OldValue,
                (WindowCornerPreference)e.NewValue
            );
        }

        /// <summary>
        /// This virtual method is called when <see cref="WindowCornerPreference"/> is changed.
        /// </summary>
        protected virtual void OnCornerPreferenceChanged(WindowCornerPreference oldValue, WindowCornerPreference newValue)
        {
            if (InteropHelper.Handle == IntPtr.Zero)
            {
                return;
            }

            _ = UnsafeNativeMethods.ApplyWindowCornerPreference(InteropHelper.Handle, newValue);
        }

        /// <summary>
        /// Private <see cref="WindowBackdropType"/> property callback.
        /// </summary>
        private static void OnWindowBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) { return; }

            if (e.OldValue == e.NewValue) { return; }

            window.OnBackdropTypeChanged((WindowBackdropType)e.OldValue, (WindowBackdropType)e.NewValue);
        }

        /// <summary>
        /// This virtual method is called when <see cref="WindowBackdropType"/> is changed.
        /// </summary>
        protected virtual void OnBackdropTypeChanged(WindowBackdropType oldValue, WindowBackdropType newValue)
        {
            if (InteropHelper.Handle == IntPtr.Zero)
            {
                return;
            }

            if (newValue == WindowBackdropType.None)
            {
                _ = WindowBackdrop.RemoveBackdrop(this);

                return;
            }

            if (!ExtendsContentIntoTitleBar)
            {
                throw new InvalidOperationException($"Cannot apply backdrop effect if {nameof(ExtendsContentIntoTitleBar)} is false.");
            }

            if (WindowBackdrop.IsSupported(newValue) && WindowBackdrop.RemoveBackground(this))
            {
                _ = WindowBackdrop.ApplyBackdrop(this, newValue);
            }
        }

        /// <summary>
        /// Private <see cref="ExtendsContentIntoTitleBar"/> property callback.
        /// </summary>
        private static void OnExtendsContentIntoTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Window window) { return; }

            if (e.OldValue == e.NewValue) { return; }

            window.OnExtendsContentIntoTitleBarChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// This virtual method is called when <see cref="ExtendsContentIntoTitleBar"/> is changed.
        /// </summary>
        protected virtual void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue)
        {
            // AllowsTransparency = true;
            SetCurrentValue(WindowStyleProperty, WindowStyle.SingleBorderWindow);

#if NET40
            var chrome = new WindowChrome
            {
                CornerRadius = new CornerRadius(),
                GlassFrameThickness = new Thickness(-1)
            };
#else
            var chrome = new WindowChrome
            {
                CaptionHeight = AreaBarHeight,
                CornerRadius = new CornerRadius(),// default
                GlassFrameThickness = new Thickness(-1),//or 0, 0, 0, 1
                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
                UseAeroCaptionButtons = false
            };
#endif
            WindowChrome.SetWindowChrome(this, chrome);

            _ = UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
        }

        /// <summary> 
        /// Listening window hooks after rendering window content to SizeToContent support
        /// </summary>
        private void OnWindowContentRendered(object? sender, EventArgs e)
        {
            if (sender is not Window window) { return; }

            window.ContentRendered -= OnWindowContentRendered;

            IntPtr handle = new WindowInteropHelper(window).Handle;
            HwndSource windowSource = HwndSource.FromHwnd(handle) ?? throw new InvalidOperationException("Window source is null");
            windowSource.AddHook(HwndSourceHook);
        }

        private IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            switch (msg)
            {
                case User32.WM_WINDOWPOSCHANGED:
                    Padding = WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : _commonPadding;
                    break;
                case User32.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(hwnd, lparam);
                    Padding = WindowState == WindowState.Maximized ? WindowHelper.WindowMaximizedPadding : _commonPadding;
                    break;
                case 0x0084:
                    // for fixing #886
                    // https://developercommunity.visualstudio.com/t/overflow-exception-in-windowchrome/167357
                    try
                    {
                        _ = lparam.ToInt32();
                    }
                    catch (OverflowException)
                    {
                        handled = true;
                    }
                    break;
            }

            return IntPtr.Zero;
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

            if (element is not T tElement) { throw new InvalidOperationException($"Template part '{name}' is not found or is not of type {typeof(T)}"); }

            return tElement;
        }
    }
}
