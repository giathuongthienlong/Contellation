using System.Windows;
using System.Windows.Controls;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// Được sử dụng để thay thế Grid
    /// </summary>
    /// <remarks>
    /// Lớp nhẹ này được khuyến nghị sử dụng khi bạn không cần các tính năng như phân tách hàng và cột trong lưới.
    /// </remarks>
    public class SimplePanel : Panel
    {
        protected override Size MeasureOverride(Size constraint)
        {
            var maxSize = new Size();

            foreach (UIElement child in InternalChildren)
            {
                if (child != null)
                {
                    child.Measure(constraint);
                    maxSize.Width = Math.Max(maxSize.Width, child.DesiredSize.Width);
                    maxSize.Height = Math.Max(maxSize.Height, child.DesiredSize.Height);
                }
            }

            return maxSize;
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (UIElement child in InternalChildren)
            {
                child?.Arrange(new Rect(arrangeSize));
            }

            return arrangeSize;
        }
    }
}
