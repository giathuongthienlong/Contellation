using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Contellation.Custom.Controls
{
    public class ResponsiveGrid : Panel
    {
        #region Dependency Properties
        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register(nameof(ShowGridLines), typeof(bool), typeof(ResponsiveGrid),
                new PropertyMetadata(false, (d, e) => ((ResponsiveGrid)d).InvalidateVisual()));

        public static readonly DependencyProperty BreakPointsProperty =
            DependencyProperty.Register(nameof(BreakPoints), typeof(BreakPoints), typeof(ResponsiveGrid),
                new PropertyMetadata(BreakPoints.Default, (d, e) => ((ResponsiveGrid)d).InvalidateMeasure()));

        public static readonly DependencyProperty ColumnGapProperty =
            DependencyProperty.Register(nameof(ColumnGap), typeof(double), typeof(ResponsiveGrid),
                new PropertyMetadata(0.0, (d, e) => ((ResponsiveGrid)d).InvalidateMeasure()));

        public static readonly DependencyProperty RowGapProperty =
            DependencyProperty.Register(nameof(RowGap), typeof(double), typeof(ResponsiveGrid),
                new PropertyMetadata(0.0, (d, e) => ((ResponsiveGrid)d).InvalidateMeasure()));

        public bool ShowGridLines { get => (bool)GetValue(ShowGridLinesProperty); set => SetValue(ShowGridLinesProperty, value); }
        public BreakPoints BreakPoints { get => (BreakPoints)GetValue(BreakPointsProperty); set => SetValue(BreakPointsProperty, value); }
        public double ColumnGap { get => (double)GetValue(ColumnGapProperty); set => SetValue(ColumnGapProperty, value); }
        public double RowGap { get => (double)GetValue(RowGapProperty); set => SetValue(RowGapProperty, value); }
        #endregion

        #region Attached Properties
        public static readonly DependencyProperty XSProperty = DependencyProperty.RegisterAttached("XS", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(12, OnLayoutChanged));
        public static readonly DependencyProperty SMProperty = DependencyProperty.RegisterAttached("SM", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));
        public static readonly DependencyProperty MDProperty = DependencyProperty.RegisterAttached("MD", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));
        public static readonly DependencyProperty LGProperty = DependencyProperty.RegisterAttached("LG", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));
        public static readonly DependencyProperty XLProperty = DependencyProperty.RegisterAttached("XL", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));

        public static readonly DependencyProperty OffsetProperty = DependencyProperty.RegisterAttached("Offset", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));
        public static readonly DependencyProperty PushProperty = DependencyProperty.RegisterAttached("Push", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));
        public static readonly DependencyProperty PullProperty = DependencyProperty.RegisterAttached("Pull", typeof(int), typeof(ResponsiveGrid), new PropertyMetadata(0, OnLayoutChanged));

        public static readonly DependencyProperty HiddenXSProperty = DependencyProperty.RegisterAttached("HiddenXS", typeof(bool), typeof(ResponsiveGrid), new PropertyMetadata(false, OnLayoutChanged));
        public static readonly DependencyProperty HiddenSMProperty = DependencyProperty.RegisterAttached("HiddenSM", typeof(bool), typeof(ResponsiveGrid), new PropertyMetadata(false, OnLayoutChanged));
        public static readonly DependencyProperty HiddenMDProperty = DependencyProperty.RegisterAttached("HiddenMD", typeof(bool), typeof(ResponsiveGrid), new PropertyMetadata(false, OnLayoutChanged));
        public static readonly DependencyProperty HiddenLGProperty = DependencyProperty.RegisterAttached("HiddenLG", typeof(bool), typeof(ResponsiveGrid), new PropertyMetadata(false, OnLayoutChanged));

        // Getters & Setters
        public static int GetXS(DependencyObject obj) => (int)obj.GetValue(XSProperty);
        public static void SetXS(DependencyObject obj, int value) => obj.SetValue(XSProperty, value);

        public static int GetSM(DependencyObject obj) => (int)obj.GetValue(SMProperty);
        public static void SetSM(DependencyObject obj, int value) => obj.SetValue(SMProperty, value);

        public static int GetMD(DependencyObject obj) => (int)obj.GetValue(MDProperty);
        public static void SetMD(DependencyObject obj, int value) => obj.SetValue(MDProperty, value);

        public static int GetLG(DependencyObject obj) => (int)obj.GetValue(LGProperty);
        public static void SetLG(DependencyObject obj, int value) => obj.SetValue(LGProperty, value);

        public static int GetXL(DependencyObject obj) => (int)obj.GetValue(XLProperty);
        public static void SetXL(DependencyObject obj, int value) => obj.SetValue(XLProperty, value);

        public static int GetOffset(DependencyObject obj) => (int)obj.GetValue(OffsetProperty);
        public static void SetOffset(DependencyObject obj, int value) => obj.SetValue(OffsetProperty, value);

        public static int GetPush(DependencyObject obj) => (int)obj.GetValue(PushProperty);
        public static void SetPush(DependencyObject obj, int value) => obj.SetValue(PushProperty, value);

        public static int GetPull(DependencyObject obj) => (int)obj.GetValue(PullProperty);
        public static void SetPull(DependencyObject obj, int value) => obj.SetValue(PullProperty, value);

        public static bool GetHiddenXS(DependencyObject obj) => (bool)obj.GetValue(HiddenXSProperty);
        public static void SetHiddenXS(DependencyObject obj, bool value) => obj.SetValue(HiddenXSProperty, value);

        public static bool GetHiddenSM(DependencyObject obj) => (bool)obj.GetValue(HiddenSMProperty);
        public static void SetHiddenSM(DependencyObject obj, bool value) => obj.SetValue(HiddenSMProperty, value);

        public static bool GetHiddenMD(DependencyObject obj) => (bool)obj.GetValue(HiddenMDProperty);
        public static void SetHiddenMD(DependencyObject obj, bool value) => obj.SetValue(HiddenMDProperty, value);

        public static bool GetHiddenLG(DependencyObject obj) => (bool)obj.GetValue(HiddenLGProperty);
        public static void SetHiddenLG(DependencyObject obj, bool value) => obj.SetValue(HiddenLGProperty, value);

        private static void OnLayoutChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && VisualTreeHelper.GetParent(element) is ResponsiveGrid grid)
                grid.InvalidateMeasure();
        }

        #endregion
        protected override Size MeasureOverride(Size availableSize)
        {
            if (double.IsPositiveInfinity(availableSize.Width))
                availableSize.Width = 1920;

            double width = availableSize.Width;
            var bp = BreakPoints;
            var visibleChildren = GetVisibleChildren(InternalChildren, width, bp);

            double totalHeight = 0;
            double currentRowHeight = 0;
            double currentX = 0;

            foreach (var child in visibleChildren)
            {
                int span = GetSpan(child, width, bp);
                double itemWidth = (width - (11 * ColumnGap)) / 12.0 * span;   // Trừ gap

                child.Measure(new Size(itemWidth, double.PositiveInfinity));
                currentRowHeight = Math.Max(currentRowHeight, child.DesiredSize.Height);

                currentX += itemWidth + ColumnGap;
                if (currentX > width * 0.98)
                {
                    totalHeight += currentRowHeight + RowGap;
                    currentRowHeight = 0;
                    currentX = 0;
                }
            }

            if (currentRowHeight > 0)
                totalHeight += currentRowHeight;

            return new Size(width, totalHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double width = finalSize.Width;
            var bp = BreakPoints;
            var visibleChildren = GetVisibleChildren(InternalChildren, width, bp);

            double currentX = 0;
            double currentY = 0;
            double rowMaxHeight = 0;

            foreach (var child in visibleChildren)
            {
                int span = GetSpan(child, width, bp);
                int offset = GetOffset(child);
                int push = GetPush(child);
                int pull = GetPull(child);

                double itemWidth = (width - (11 * ColumnGap)) / 12.0 * span;
                double finalX = currentX + (offset + push - pull) * ((width - (11 * ColumnGap)) / 12.0);

                if (finalX + itemWidth > width * 1.02)
                {
                    currentY += rowMaxHeight + RowGap;
                    currentX = 0;
                    rowMaxHeight = 0;
                    finalX = currentX + (offset + push - pull) * ((width - (11 * ColumnGap)) / 12.0);
                }

                child.Arrange(new Rect(finalX, currentY, itemWidth, child.DesiredSize.Height));

                rowMaxHeight = Math.Max(rowMaxHeight, child.DesiredSize.Height);
                currentX += itemWidth + ColumnGap;
            }

            return finalSize;
        }

        private List<UIElement> GetVisibleChildren(UIElementCollection children, double width, BreakPoints bp)
        {
            return children.OfType<UIElement>()
                .Where(child => IsVisibleAtBreakpoint(child, width, bp))
                .ToList();
        }

        private bool IsVisibleAtBreakpoint(UIElement element, double width, BreakPoints bp)
        {
            if (GetHiddenXS(element) && width < bp.SM) return false;
            if (GetHiddenSM(element) && width >= bp.SM && width < bp.MD) return false;
            if (GetHiddenMD(element) && width >= bp.MD && width < bp.LG) return false;
            if (GetHiddenLG(element) && width >= bp.LG) return false;
            return true;
        }

        private int GetSpan(UIElement element, double width, BreakPoints bp)
        {
            if (width >= bp.XL && GetXL(element) > 0) return GetXL(element);
            if (width >= bp.LG && GetLG(element) > 0) return GetLG(element);
            if (width >= bp.MD && GetMD(element) > 0) return GetMD(element);
            if (width >= bp.SM && GetSM(element) > 0) return GetSM(element);
            return GetXS(element);
        }

        //private int GetOffset(UIElement element) => (int)element.GetValue(OffsetProperty);
        private int GetOffset(UIElement element, double width, BreakPoints bp)
        {
            return GetOffset(element); // có thể mở rộng responsive offset sau
        }

        // ==================== GRID LINES ====================
        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (!ShowGridLines) return;

            // Grid lines + Breakpoints (giữ nguyên code vẽ từ trước)
            var pen = new Pen(Brushes.Red, 1) { DashStyle = DashStyles.Dash };
            var bpPen = new Pen(Brushes.Orange, 2) { DashStyle = DashStyles.Dot };
            double w = ActualWidth;
            double h = ActualHeight;

            for (int i = 1; i < 12; i++)
                dc.DrawLine(pen, new Point((w / 12) * i, 0), new Point((w / 12) * i, h));

            // Breakpoint lines...
            var bp = BreakPoints;
            var points = new[] { bp.SM, bp.MD, bp.LG, bp.XL };
            var labels = new[] { "SM", "MD", "LG", "XL" };

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] > 0 && points[i] < w)
                {
                    dc.DrawLine(bpPen, new Point(points[i], 0), new Point(points[i], h));
                    var ft = new FormattedText(labels[i], CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                        new Typeface("Segoe UI"), 10, Brushes.Orange, 1.0);
                    dc.DrawText(ft, new Point(points[i] + 4, 4));
                }
            }
        }

    }
}
