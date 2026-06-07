using Contellation.Custom.Enums.Control;
using Contellation.Custom.Extensions;
using Contellation.Custom.Interops;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Media;

namespace Contellation.Custom.Controls
{
    public class Button : System.Windows.Controls.Button
    {
        /// <inheritdoc />
        [Bindable(true)]
        [Category("Appearance")]
        public ControlAppearance Appearance
        {
            get => (ControlAppearance)GetValue(AppearanceProperty);
            set => SetValue(AppearanceProperty, value);
        }
        public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(
            nameof(Appearance),
            typeof(ControlAppearance),
            typeof(Button),
            new PropertyMetadata(ControlAppearance.Primary)
        );

        /// <summary>
        /// Gets or sets background <see cref="Brush"/>.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush MouseOverBackground
        {
            get => (Brush)GetValue(MouseOverBackgroundProperty);
            set => SetValue(MouseOverBackgroundProperty, value);
        }
        public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register(
            nameof(MouseOverBackground),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue)
        );

        /// <summary>
        /// Gets or sets border <see cref="Brush"/> when the user mouses over the button.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush MouseOverBorderBrush
        {
            get => (Brush)GetValue(MouseOverBorderBrushProperty);
            set => SetValue(MouseOverBorderBrushProperty, value);
        }
        public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register(
            nameof(MouseOverBorderBrush),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue)
        );

        /// <summary>
        /// Gets or sets the foreground <see cref="Brush"/> when the user clicks the button.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush PressedForeground
        {
            get => (Brush)GetValue(PressedForegroundProperty);
            set => SetValue(PressedForegroundProperty, value);
        }
        public static readonly DependencyProperty PressedForegroundProperty = DependencyProperty.Register(
            nameof(PressedForeground),
            typeof(Brush),
            typeof(Button),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits
            )
        );

        /// <summary>
        /// Gets or sets background <see cref="Brush"/> when the user clicks the button.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush PressedBackground
        {
            get => (Brush)GetValue(PressedBackgroundProperty);
            set => SetValue(PressedBackgroundProperty, value);
        }
        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(
            nameof(PressedBackground),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue)
        );

        /// <summary>
        /// Gets or sets border <see cref="Brush"/> when the user clicks the button.
        /// </summary>
        [Bindable(true)]
        [Category("Appearance")]
        public Brush PressedBorderBrush
        {
            get => (Brush)GetValue(PressedBorderBrushProperty);
            set => SetValue(PressedBorderBrushProperty, value);
        }
        public static readonly DependencyProperty PressedBorderBrushProperty = DependencyProperty.Register(
            nameof(PressedBorderBrush),
            typeof(Brush),
            typeof(Button),
            new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue)
        );

        /// <summary>
        /// Gets or sets a value that represents the degree to which the corners of a <see cref="T:System.Windows.Controls.Border" /> are rounded.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, (object)value);
        }
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(Button),
            new FrameworkPropertyMetadata(
                default(CornerRadius),
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender
            )
        );

        //
        /// <summary>
        /// Gets or sets the type of the button.
        /// </summary>
        public ButtonAction ButtonType
        {
            get => (ButtonAction)GetValue(ButtonTypeProperty);
            set => SetValue(ButtonTypeProperty, value);
        }
        /// <summary>Identifies the <see cref="ButtonType"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonTypeProperty = DependencyProperty.Register(
            nameof(ButtonType),
            typeof(ButtonAction),
            typeof(Button),
            new PropertyMetadata(ButtonAction.Unknown, OnButtonTypeChanged)
        );

        /// <summary>
        /// Gets or sets the foreground of the navigation buttons.
        /// </summary>
        public Brush ButtonsForeground
        {
            get => (Brush)GetValue(ButtonsForegroundProperty);
            set => SetValue(ButtonsForegroundProperty, value);
        }
        /// <summary>Identifies the <see cref="ButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty ButtonsForegroundProperty = DependencyProperty.Register(
            nameof(ButtonsForeground),
            typeof(Brush),
            typeof(Button),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits
            )
        );

        /// <summary>
        /// Gets or sets the foreground of the navigation buttons when moused over.
        /// </summary>
        public Brush? MouseOverButtonsForeground
        {
            get => (Brush?)GetValue(MouseOverButtonsForegroundProperty);
            set => SetValue(MouseOverButtonsForegroundProperty, value);
        }
        /// <summary>Identifies the <see cref="MouseOverButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty MouseOverButtonsForegroundProperty =
            DependencyProperty.Register(
                nameof(MouseOverButtonsForeground),
                typeof(Brush),
                typeof(Button),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits)
            );

        public Brush RenderButtonsForeground
        {
            get => (Brush)GetValue(RenderButtonsForegroundProperty);
            set => SetValue(RenderButtonsForegroundProperty, value);
        }
        /// <summary>Identifies the <see cref="RenderButtonsForeground"/> dependency property.</summary>
        public static readonly DependencyProperty RenderButtonsForegroundProperty = DependencyProperty.Register(
            nameof(RenderButtonsForeground),
            typeof(Brush),
            typeof(Button),
            new FrameworkPropertyMetadata(
                SystemColors.ControlTextBrush,
                FrameworkPropertyMetadataOptions.Inherits
            )
        );

        public bool IsHovered { get; private set; }

        private readonly Brush _defaultBackgroundBrush = Brushes.Transparent; // REVIEW: Should it be transparent?
        private uint _returnValue;

        private bool _isClickedDown;

        public Button()
        {
            Loaded += Button_Loaded;
            Unloaded += Button_Unloaded;
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);
            DependencyPropertyDescriptor
                .FromProperty(ButtonsForegroundProperty, typeof(Brush))
                .AddValueChanged(this, OnButtonsForegroundChanged);
        }


        private void Button_Unloaded(object sender, RoutedEventArgs e)
        {
            DependencyPropertyDescriptor
                .FromProperty(ButtonsForegroundProperty, typeof(Brush))
                .RemoveValueChanged(this, OnButtonsForegroundChanged);
        }

        private void OnButtonsForegroundChanged(object? sender, EventArgs e)
        {
            SetCurrentValue(
                RenderButtonsForegroundProperty,
                IsHovered ? MouseOverButtonsForeground : ButtonsForeground
            );
        }

        /// <summary>
        /// Forces button background to change.
        /// </summary>
        public void Hover()
        {
            if (IsHovered)
            {
                return;
            }

            SetCurrentValue(BackgroundProperty, MouseOverBackground);
            if (MouseOverButtonsForeground != null)
            {
                SetCurrentValue(RenderButtonsForegroundProperty, MouseOverButtonsForeground);
            }

            IsHovered = true;
        }

        /// <summary>
        /// Forces button background to change.
        /// </summary>
        public void RemoveHover()
        {
            if (!IsHovered)
            {
                return;
            }

            SetCurrentValue(BackgroundProperty, _defaultBackgroundBrush);
            SetCurrentValue(RenderButtonsForegroundProperty, ButtonsForeground);

            IsHovered = false;
            _isClickedDown = false;
        }

        /// <summary>
        /// Invokes click on the button.
        /// </summary>
        public void InvokeClick()
        {
            if (
                new ButtonAutomationPeer(this).GetPattern(PatternInterface.Invoke)
                is IInvokeProvider invokeProvider
            )
            {
                invokeProvider.Invoke();
            }

            _isClickedDown = false;
        }

        //internal bool ReactToHwndHook(uint msg, IntPtr lParam, out IntPtr returnIntPtr)
        internal bool ReactToHwndHook(User32.WM msg, IntPtr lParam, out IntPtr returnIntPtr)
        {
            returnIntPtr = IntPtr.Zero;

            switch (msg)
            {
                case User32.WM.NCHITTEST:
                    if (this.IsMouseOverElement(lParam))
                    {
                        /*Debug.WriteLine($"Hitting {ButtonType} | return code {_returnValue}");*/
                        Hover();
                        returnIntPtr = (IntPtr)_returnValue;
                        return true;
                    }

                    RemoveHover();
                    return false;
                case User32.WM.NCMOUSELEAVE: // Mouse leaves the window
                    RemoveHover();
                    return false;
                case User32.WM.NCLBUTTONDOWN when this.IsMouseOverElement(lParam): // Left button clicked down
                    _isClickedDown = true;
                    return true;
                case User32.WM.NCLBUTTONUP when _isClickedDown && this.IsMouseOverElement(lParam): // Left button clicked up
                    InvokeClick();
                    return true;
                default:
                    return false;
            }
        }

        private static void OnButtonTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not Button titleBarButton)
            {
                return;
            }

            titleBarButton.OnButtonTypeChanged(e);
        }

        protected void OnButtonTypeChanged(DependencyPropertyChangedEventArgs e)
        {
            var buttonType = (ButtonAction)e.NewValue;

            _returnValue = buttonType switch
            {
                ButtonAction.Unknown => (uint)User32.WM_NCHITTEST.HTNOWHERE,
                ButtonAction.Help => (uint)User32.WM_NCHITTEST.HTHELP,
                ButtonAction.Minimize => (uint)User32.WM_NCHITTEST.HTMINBUTTON,
                ButtonAction.Close => (uint)User32.WM_NCHITTEST.HTCLOSE,
                ButtonAction.Restore => (uint)User32.WM_NCHITTEST.HTMAXBUTTON,
                ButtonAction.Maximize => (uint)User32.WM_NCHITTEST.HTMAXBUTTON,
                _ => throw new ArgumentOutOfRangeException(
                    "e.NewValue",
                    buttonType,
                    $"Unsupported button type: {buttonType}."
                ),
            };
        }
    }
}
