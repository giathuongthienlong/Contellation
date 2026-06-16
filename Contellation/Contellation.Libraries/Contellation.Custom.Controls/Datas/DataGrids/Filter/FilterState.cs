using System.Windows;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// Attached Property "FilterState" to Filter Button
    /// </summary>
    public class FilterState : DependencyObject
    {

        public static readonly DependencyProperty IsFilteredProperty = DependencyProperty.RegisterAttached("IsFiltered",
            typeof(bool), typeof(FilterState), new UIPropertyMetadata(false));

        public static bool GetIsFiltered(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsFilteredProperty);
        }

        public static void SetIsFiltered(DependencyObject obj, bool value)
        {
            obj.SetValue(IsFilteredProperty, value);
        }
    }
}
