using System.Windows;

namespace Contellation.Custom.Controls
{

    public partial class ResponsiveGridss
    {
        //public ResponsiveGrid()
        //{
        //    this.MaxDivision = 12;
        //    this.BreakPoints = new BreakPoints();
        //}

        //protected override Size MeasureOverride(Size availableSize)
        //{
        //    var count = 0;
        //    var currentRow = 0;

        //    var availableWidth = double.IsPositiveInfinity(availableSize.Width) ? double.PositiveInfinity : availableSize.Width / this.MaxDivision;
        //    var children = this.Children.OfType<UIElement>();


        //    foreach (UIElement child in this.Children)
        //    {
        //        if (child != null)
        //        {
        //            // Không bố trí khi đã thu gọn 
        //            if (child.Visibility == Visibility.Collapsed) { continue; }

        //            var span = this.GetSpan(child, availableSize.Width);
        //            var offset = this.GetOffset(child, availableSize.Width);
        //            var push = this.GetPush(child, availableSize.Width);
        //            var pull = this.GetPull(child, availableSize.Width);

        //            if (count + span + offset > this.MaxDivision)
        //            {
        //                // cài lại
        //                currentRow++;
        //                count = 0;
        //            }

        //            SetActualColumn(child, count + offset + push - pull);
        //            SetActualRow(child, currentRow);

        //            count += (span + offset);

        //            var size = new Size(availableWidth * span, double.PositiveInfinity);
        //            child.Measure(size);
        //        }
        //    }

        //    //  Nhóm theo hàng
        //    var group = this.Children.OfType<UIElement>().GroupBy(x => GetActualRow(x));

        //    var totalSize = new Size();
        //    if (group.Count() != 0)
        //    {
        //        totalSize.Width = group.Max(rows => rows.Sum(o => o.DesiredSize.Width));
        //        totalSize.Height = group.Sum(rows => rows.Max(o => o.DesiredSize.Height));
        //    }

        //    return totalSize;
        //}

        //protected int GetSpan(UIElement element, double width)
        //{
        //    var span = 0;

        //    var getXS = new Func<UIElement, int>((o) => { var x = GetXS(o); return x != 0 ? x : this.MaxDivision; });
        //    var getSM = new Func<UIElement, int>((o) => { var x = GetSM(o); return x != 0 ? x : getXS(o); });
        //    var getMD = new Func<UIElement, int>((o) => { var x = GetMD(o); return x != 0 ? x : getSM(o); });
        //    var getLG = new Func<UIElement, int>((o) => { var x = GetLG(o); return x != 0 ? x : getMD(o); });

        //    if (width < this.BreakPoints.XS_SM) { span = getXS(element); }
        //    else if (width < this.BreakPoints.SM_MD) { span = getSM(element); }
        //    else if (width < this.BreakPoints.MD_LG) { span = getMD(element); }
        //    else { span = getLG(element); }

        //    return Math.Min(Math.Max(0, span), this.MaxDivision); ;
        //}

        //protected int GetOffset(UIElement element, double width)
        //{
        //    var span = 0;

        //    var getXS = new Func<UIElement, int>((o) => { var x = GetXS_Offset(o); return x != 0 ? x : 0; });
        //    var getSM = new Func<UIElement, int>((o) => { var x = GetSM_Offset(o); return x != 0 ? x : getXS(o); });
        //    var getMD = new Func<UIElement, int>((o) => { var x = GetMD_Offset(o); return x != 0 ? x : getSM(o); });
        //    var getLG = new Func<UIElement, int>((o) => { var x = GetLG_Offset(o); return x != 0 ? x : getMD(o); });

        //    if (width < this.BreakPoints.XS_SM) { span = getXS(element); }
        //    else if (width < this.BreakPoints.SM_MD) { span = getSM(element); }
        //    else if (width < this.BreakPoints.MD_LG) { span = getMD(element); }
        //    else { span = getLG(element); }

        //    return Math.Min(Math.Max(0, span), this.MaxDivision); ;
        //}

        //protected int GetPush(UIElement element, double width)
        //{
        //    var span = 0;

        //    var getXS = new Func<UIElement, int>((o) => { var x = GetXS_Push(o); return x != 0 ? x : 0; });
        //    var getSM = new Func<UIElement, int>((o) => { var x = GetSM_Push(o); return x != 0 ? x : getXS(o); });
        //    var getMD = new Func<UIElement, int>((o) => { var x = GetMD_Push(o); return x != 0 ? x : getSM(o); });
        //    var getLG = new Func<UIElement, int>((o) => { var x = GetLG_Push(o); return x != 0 ? x : getMD(o); });

        //    if (width < this.BreakPoints.XS_SM) { span = getXS(element); }
        //    else if (width < this.BreakPoints.SM_MD) { span = getSM(element); }
        //    else if (width < this.BreakPoints.MD_LG) { span = getMD(element); }
        //    else { span = getLG(element); }

        //    return Math.Min(Math.Max(0, span), this.MaxDivision); ;
        //}

        //protected int GetPull(UIElement element, double width)
        //{
        //    var span = 0;

        //    var getXS = new Func<UIElement, int>((o) => { var x = GetXS_Pull(o); return x != 0 ? x : 0; });
        //    var getSM = new Func<UIElement, int>((o) => { var x = GetSM_Pull(o); return x != 0 ? x : getXS(o); });
        //    var getMD = new Func<UIElement, int>((o) => { var x = GetMD_Pull(o); return x != 0 ? x : getSM(o); });
        //    var getLG = new Func<UIElement, int>((o) => { var x = GetLG_Pull(o); return x != 0 ? x : getMD(o); });

        //    if (width < this.BreakPoints.XS_SM) { span = getXS(element); }
        //    else if (width < this.BreakPoints.SM_MD) { span = getSM(element); }
        //    else if (width < this.BreakPoints.MD_LG) { span = getMD(element); }
        //    else { span = getLG(element); }

        //    return Math.Min(Math.Max(0, span), this.MaxDivision); ;
        //}

        //protected override Size ArrangeOverride(Size finalSize)
        //{
        //    var columnWidth = finalSize.Width / this.MaxDivision;

        //    // Nhóm theo hàng
        //    var group = this.Children.OfType<UIElement>().GroupBy(x => GetActualRow(x));

        //    double temp = 0;
        //    foreach (var rows in group)
        //    {
        //        double max = 0;

        //        var columnHeight = rows.Max(o => o.DesiredSize.Height);
        //        foreach (var element in rows)
        //        {
        //            var column = GetActualColumn(element);
        //            var row = GetActualRow(element);
        //            var columnSpan = this.GetSpan(element, finalSize.Width);

        //            var rect = new Rect(columnWidth * column, temp, columnWidth * columnSpan, columnHeight);

        //            element.Arrange(rect);

        //            max = Math.Max(element.DesiredSize.Height, max);
        //        }

        //        temp += max;
        //    }
        //    return base.ArrangeOverride(finalSize);
        //}

        //private static readonly Pen _guidePen1 = new Pen(Brushes.Yellow, 1);
        //private static readonly Pen _guidePen2 = new Pen(Brushes.Blue, 1)
        //{
        //    DashStyle = new DashStyle(new double[] { 4, 4 }, 0)
        //};

        //protected override void OnRender(DrawingContext dc)
        //{
        //    base.OnRender(dc);
        //    // ShowGridLines (Nếu được bật, hãy vẽ lưới hướng dẫn trước khi vẽ các phần tử khác nhau để dễ thấy)
        //    if (this.ShowGridLines)
        //    {
        //        var gridNum = this.MaxDivision;
        //        var unit = this.ActualWidth / gridNum;
        //        for (var i = 0; i <= gridNum; i++)
        //        {
        //            var x = (int)(unit * i);
        //            dc.DrawLine(_guidePen1, new Point(x, 0), new Point(x, this.ActualHeight));
        //            dc.DrawLine(_guidePen2, new Point(x, 0), new Point(x, this.ActualHeight));
        //        }
        //    }
        //}
        #region ResponsiveGrid Thuộc tính phụ thuộc để tự thiết lập các điểm dừng khác nhau

        ///// <summary>
        ///// Type int
        ///// Gets or sets a value that determines grid divisions.
        ///// </summary>
        //public int MaxDivision
        //{
        //    get { return (int)GetValue(MaxDivisionProperty); }
        //    set { SetValue(MaxDivisionProperty, value); }
        //}
        //public static readonly DependencyProperty MaxDivisionProperty = DependencyProperty.Register(nameof(MaxDivision), typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(12, FrameworkPropertyMetadataOptions.AffectsMeasure));

        //#endregion

        //public BreakPoints BreakPoints
        //{
        //    get { return (BreakPoints)GetValue(BreakPointsProperty); }
        //    set { SetValue(BreakPointsProperty, value); }
        //}
        //public static readonly DependencyProperty BreakPointsProperty = DependencyProperty.Register(nameof(BreakPoints), typeof(BreakPoints),
        //        typeof(ResponsiveGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));

        ///// <summary>
        ///// Type int
        ///// Gets or sets a value that indicates whether grid column's lines are visible within this ResponsiveGrid.
        ///// </summary>
        //public bool ShowGridLines
        //{
        //    get { return (bool)GetValue(ShowGridLinesProperty); }
        //    set { SetValue(ShowGridLinesProperty, value); }
        //}
        //// Using a DependencyProperty as the backing store for ShowGridLines. This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ShowGridLinesProperty = DependencyProperty.Register(nameof(ShowGridLines), typeof(bool),
        //        typeof(ResponsiveGrid), new PropertyMetadata(false));


        //#region  Thuộc tính đính kèm để xác định kích thước của từng phần tử con
        //public static int GetXS(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(XSProperty);
        //}
        //public static void SetXS(DependencyObject obj, int value)
        //{
        //    obj.SetValue(XSProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns for XS(extra small)
        ///// </summary>
        //public static readonly DependencyProperty XSProperty = DependencyProperty.RegisterAttached("XS", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetSM(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(SMProperty);
        //}
        //public static void SetSM(DependencyObject obj, int value)
        //{
        //    obj.SetValue(SMProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns for SM(small)
        ///// </summary>
        //public static readonly DependencyProperty SMProperty = DependencyProperty.RegisterAttached("SM", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetMD(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(MDProperty);
        //}
        //public static void SetMD(DependencyObject obj, int value)
        //{
        //    obj.SetValue(MDProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns for MD(medium)
        ///// </summary>
        //public static readonly DependencyProperty MDProperty = DependencyProperty.RegisterAttached("MD", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetLG(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(LGProperty);
        //}
        //public static void SetLG(DependencyObject obj, int value)
        //{
        //    obj.SetValue(LGProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns for LG(large)
        ///// </summary>
        //public static readonly DependencyProperty LGProperty = DependencyProperty.RegisterAttached("LG", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));


        //#endregion

        //#region Offset Property
        //public static int GetXS_Offset(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(XS_OffsetProperty);
        //}
        //public static void SetXS_Offset(DependencyObject obj, int value)
        //{
        //    obj.SetValue(XS_OffsetProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns offset for XS(extra small)
        ///// </summary>
        //public static readonly DependencyProperty XS_OffsetProperty = DependencyProperty.RegisterAttached("XS_Offset", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetSM_Offset(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(SM_OffsetProperty);
        //}
        //public static void SetSM_Offset(DependencyObject obj, int value)
        //{
        //    obj.SetValue(SM_OffsetProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns offset for SM(small)
        ///// </summary>
        //public static readonly DependencyProperty SM_OffsetProperty = DependencyProperty.RegisterAttached("SM_Offset", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetMD_Offset(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(MD_OffsetProperty);
        //}
        //public static void SetMD_Offset(DependencyObject obj, int value)
        //{
        //    obj.SetValue(MD_OffsetProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns offset for MD(medium)
        ///// </summary>
        //public static readonly DependencyProperty MD_OffsetProperty = DependencyProperty.RegisterAttached("MD_Offset", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetLG_Offset(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(LG_OffsetProperty);
        //}
        //public static void SetLG_Offset(DependencyObject obj, int value)
        //{
        //    obj.SetValue(LG_OffsetProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that determines grid columns offset for LG(large)
        ///// </summary>
        //public static readonly DependencyProperty LG_OffsetProperty = DependencyProperty.RegisterAttached("LG_Offset", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //#endregion

        //#region Push Property 
        //public static int GetXS_Push(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(XS_PushProperty);
        //}
        //public static void SetXS_Push(DependencyObject obj, int value)
        //{
        //    obj.SetValue(XS_PushProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to right from the original position XS.
        ///// </summary>
        //public static readonly DependencyProperty XS_PushProperty = DependencyProperty.RegisterAttached("XS_Push", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetSM_Push(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(SM_PushProperty);
        //}
        //public static void SetSM_Push(DependencyObject obj, int value)
        //{
        //    obj.SetValue(SM_PushProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to right from the original position SM.
        ///// </summary>
        //public static readonly DependencyProperty SM_PushProperty = DependencyProperty.RegisterAttached("SM_Push", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));


        //public static int GetMD_Push(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(MD_PushProperty);
        //}
        //public static void SetMD_Push(DependencyObject obj, int value)
        //{
        //    obj.SetValue(MD_PushProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to right from the original position MD.
        ///// </summary>
        //public static readonly DependencyProperty MD_PushProperty = DependencyProperty.RegisterAttached("MD_Push", typeof(int),
        //        typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetLG_Push(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(LG_PushProperty);
        //}
        //public static void SetLG_Push(DependencyObject obj, int value)
        //{
        //    obj.SetValue(LG_PushProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to right from the original position LG.
        ///// </summary>
        //public static readonly DependencyProperty LG_PushProperty = DependencyProperty.RegisterAttached("LG_Push", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //#endregion

        //#region  Pull Property
        //public static int GetXS_Pull(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(XS_PullProperty);
        //}
        //public static void SetXS_Pull(DependencyObject obj, int value)
        //{
        //    obj.SetValue(XS_PullProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to left from the original position XS.
        ///// </summary>
        //public static readonly DependencyProperty XS_PullProperty = DependencyProperty.RegisterAttached("XS_Pull", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetSM_Pull(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(SM_PullProperty);
        //}
        //public static void SetSM_Pull(DependencyObject obj, int value)
        //{
        //    obj.SetValue(SM_PullProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to left from the original position SM.
        ///// </summary>
        //public static readonly DependencyProperty SM_PullProperty = DependencyProperty.RegisterAttached("SM_Pull", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetMD_Pull(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(MD_PullProperty);
        //}
        //public static void SetMD_Pull(DependencyObject obj, int value)
        //{
        //    obj.SetValue(MD_PullProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to left from the original position MD.
        ///// </summary>
        //public static readonly DependencyProperty MD_PullProperty = DependencyProperty.RegisterAttached("MD_Pull", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //public static int GetLG_Pull(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(LG_PullProperty);
        //}
        //public static void SetLG_Pull(DependencyObject obj, int value)
        //{
        //    obj.SetValue(LG_PullProperty, value);
        //}
        ///// <summary>
        ///// Gets or sets a value that moves columns to left from the original position LG.
        ///// </summary>
        //public static readonly DependencyProperty LG_PullProperty = DependencyProperty.RegisterAttached("LG_Pull", typeof(int),
        //    typeof(ResponsiveGrid), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        //#endregion

        //#region Attached properties read-only
        //public static int GetActualColumn(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(ActualColumnProperty);
        //}
        //protected static void SetActualColumn(DependencyObject obj, int value)
        //{
        //    obj.SetValue(ActualColumnProperty, value);
        //}
        //public static readonly DependencyProperty ActualColumnProperty = DependencyProperty.RegisterAttached("ActualColumn", typeof(int),
        //    typeof(ResponsiveGrid), new PropertyMetadata(0));

        //public static int GetActualRow(DependencyObject obj)
        //{
        //    return (int)obj.GetValue(ActualRowProperty);
        //}
        //protected static void SetActualRow(DependencyObject obj, int value)
        //{
        //    obj.SetValue(ActualRowProperty, value);
        //}
        //public static readonly DependencyProperty ActualRowProperty = DependencyProperty.RegisterAttached("ActualRow", typeof(int),
        //    typeof(ResponsiveGrid), new PropertyMetadata(0));

        #endregion
    }
}
