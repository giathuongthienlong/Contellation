using System;
using System.Collections.Generic;
using System.Text;

namespace Contellation.Custom.Controls
{
    //class Test : TextBox
    //{
    //    private bool _valueUpdating;

    //    #region Dependency Properties

    //    public double? Value
    //    {
    //        get => (double?)GetValue(ValueProperty);
    //        set => SetValue(ValueProperty, value);
    //    }

    //    public static readonly DependencyProperty ValueProperty =
    //        DependencyProperty.Register(nameof(Value), typeof(double?), typeof(NumberBox),
    //            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

    //    public double Minimum
    //    {
    //        get => (double)GetValue(MinimumProperty);
    //        set => SetValue(MinimumProperty, value);
    //    }

    //    public static readonly DependencyProperty MinimumProperty =
    //        DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(NumberBox),
    //            new PropertyMetadata(double.MinValue));

    //    public double Maximum
    //    {
    //        get => (double)GetValue(MaximumProperty);
    //        set => SetValue(MaximumProperty, value);
    //    }

    //    public static readonly DependencyProperty MaximumProperty =
    //        DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(NumberBox),
    //            new PropertyMetadata(double.MaxValue));

    //    public double SmallChange
    //    {
    //        get => (double)GetValue(SmallChangeProperty);
    //        set => SetValue(SmallChangeProperty, value);
    //    }

    //    public static readonly DependencyProperty SmallChangeProperty =
    //        DependencyProperty.Register(nameof(SmallChange), typeof(double), typeof(NumberBox),
    //            new PropertyMetadata(1.0));

    //    public double LargeChange
    //    {
    //        get => (double)GetValue(LargeChangeProperty);
    //        set => SetValue(LargeChangeProperty, value);
    //    }

    //    public static readonly DependencyProperty LargeChangeProperty =
    //        DependencyProperty.Register(nameof(LargeChange), typeof(double), typeof(NumberBox),
    //            new PropertyMetadata(10.0));

    //    public int MaxDecimalPlaces
    //    {
    //        get => (int)GetValue(MaxDecimalPlacesProperty);
    //        set => SetValue(MaxDecimalPlacesProperty, value);
    //    }

    //    public static readonly DependencyProperty MaxDecimalPlacesProperty =
    //        DependencyProperty.Register(nameof(MaxDecimalPlaces), typeof(int), typeof(NumberBox),
    //            new PropertyMetadata(2, OnFormatChanged));

    //    public bool ClearButtonEnabled
    //    {
    //        get => (bool)GetValue(ClearButtonEnabledProperty);
    //        set => SetValue(ClearButtonEnabledProperty, value);
    //    }

    //    public static readonly DependencyProperty ClearButtonEnabledProperty =
    //        DependencyProperty.Register(nameof(ClearButtonEnabled), typeof(bool), typeof(NumberBox),
    //            new PropertyMetadata(true));

    //    #endregion

    //    #region Routed Events

    //    public static readonly RoutedEvent EnterPressedEvent =
    //        EventManager.RegisterRoutedEvent(nameof(EnterPressed), RoutingStrategy.Bubble,
    //            typeof(RoutedEventHandler), typeof(NumberBox));

    //    public event RoutedEventHandler EnterPressed
    //    {
    //        add => AddHandler(EnterPressedEvent, value);
    //        remove => RemoveHandler(EnterPressedEvent, value);
    //    }

    //    public static readonly RoutedCommand IncrementCommand = new RoutedCommand("Increment", typeof(NumberBox));
    //    public static readonly RoutedCommand DecrementCommand = new RoutedCommand("Decrement", typeof(NumberBox));


    //    #endregion

    //    static NumberBox()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberBox),
    //            new FrameworkPropertyMetadata(typeof(NumberBox)));
    //    }

    //    public NumberBox()
    //    {
    //        HorizontalContentAlignment = HorizontalAlignment.Right;
    //        Loaded += (s, e) => UpdateTextToValue();

    //        CommandBindings.Add(new CommandBinding(IncrementCommand, (s, e) => Increment()));
    //        CommandBindings.Add(new CommandBinding(DecrementCommand, (s, e) => Decrement()));
    //    }

    //    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (d is NumberBox nb && !nb._valueUpdating)
    //        {
    //            nb._valueUpdating = true;
    //            nb.UpdateTextToValue();
    //            nb._valueUpdating = false;
    //        }
    //    }

    //    #region Key Events

    //    protected override void OnKeyDown(KeyEventArgs e)
    //    {
    //        base.OnKeyDown(e);

    //        switch (e.Key)
    //        {
    //            case Key.Enter:
    //                UpdateValueToText();                    // Xác nhận giá trị
    //                RaiseEvent(new RoutedEventArgs(EnterPressedEvent));
    //                e.Handled = true;
    //                break;

    //            case Key.Up:
    //                Increment();
    //                e.Handled = true;
    //                break;

    //            case Key.Down:
    //                Decrement();
    //                e.Handled = true;
    //                break;

    //            case Key.PageUp:
    //                StepValue(LargeChange);
    //                e.Handled = true;
    //                break;

    //            case Key.PageDown:
    //                StepValue(-LargeChange);
    //                e.Handled = true;
    //                break;
    //        }
    //    }

    //    protected override void OnKeyUp(KeyEventArgs e)
    //    {
    //        base.OnKeyUp(e);

    //        // Có thể thêm logic tùy chỉnh khi KeyUp nếu cần
    //        // Ví dụ: Validate realtime
    //    }

    //    #endregion

    //    private static void OnFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (d is NumberBox nb)
    //            nb.UpdateTextToValue();
    //    }

    //    private void UpdateTextToValue()
    //    {
    //        if (Value.HasValue)
    //        {
    //            string format = MaxDecimalPlaces > 0 ? $"N{MaxDecimalPlaces}" : "N0";
    //            Text = Value.Value.ToString(format, CultureInfo.CurrentCulture);
    //        }
    //        else
    //        {
    //            Text = string.Empty;
    //        }
    //    }

    //    private void UpdateValueToText()
    //    {
    //        if (string.IsNullOrWhiteSpace(Text))
    //        {
    //            SetCurrentValue(ValueProperty, null);
    //            return;
    //        }

    //        if (double.TryParse(Text, NumberStyles.Any, CultureInfo.CurrentCulture, out double result))
    //        {
    //            result = Math.Max(Minimum, Math.Min(Maximum, result));
    //            SetCurrentValue(ValueProperty, result);
    //        }
    //        else
    //        {
    //            UpdateTextToValue(); // Khôi phục giá trị cũ
    //        }
    //    }

    //    protected override void OnLostFocus(RoutedEventArgs e)
    //    {
    //        base.OnLostFocus(e);
    //        UpdateValueToText();
    //    }

    //    protected override void OnPreviewTextInput(TextCompositionEventArgs e)
    //    {
    //        // Cho phép số, dấu chấm, dấu phẩy, dấu trừ
    //        if (!"0123456789.,-".Contains(e.Text))
    //            e.Handled = true;

    //        base.OnPreviewTextInput(e);
    //    }

    //    // Hỗ trợ Spin Button (nếu có trong template)
    //    public void Increment() => StepValue(SmallChange);
    //    public void Decrement() => StepValue(-SmallChange);

    //    private void StepValue(double change)
    //    {
    //        double newValue = (Value ?? 0) + change;
    //        newValue = Math.Max(Minimum, Math.Min(Maximum, newValue));
    //        SetCurrentValue(ValueProperty, newValue);
    //    }
    //}
}
