using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Contellation.Custom.Controls
{
    public class Element
    {
        /// <summary> Placeholder </summary>
        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.RegisterAttached("Placeholder",
            typeof(string), typeof(Element), new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));
        public static void SetPlaceholder(DependencyObject element, string value) => element.SetValue(PlaceholderProperty, value);
        public static string GetPlaceholder(DependencyObject element) => (string)element.GetValue(PlaceholderProperty);

        /// <summary> Icon </summary>
        public static readonly DependencyProperty IconProperty = DependencyProperty.RegisterAttached("Icon",
            typeof(string), typeof(Element), new PropertyMetadata(null));
        public static void SetIcon(DependencyObject element, string value) => element.SetValue(IconProperty, value);
        public static string GetIcon(DependencyObject element) => (string)element.GetValue(IconProperty);

        /// <summary> FontFamily FontAwesomes for Icon <see cref="FontFamily"/>. </summary>
        public static readonly DependencyProperty FontFamilyOtherProperty = DependencyProperty.RegisterAttached("FontFamilyOther",
            typeof(FontFamily), typeof(Element), new PropertyMetadata(null));
        public static void SetFontFamilyIcon(DependencyObject element, FontFamily value) => element.SetValue(FontFamilyOtherProperty, value);
        public static FontFamily GetFontFamilyIcon(DependencyObject element) => (FontFamily)element.GetValue(FontFamilyOtherProperty);

        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.RegisterAttached("IsReadOnly",
            typeof(bool), typeof(Element), new PropertyMetadata(false));

        public static void SetIsReadOnly(DependencyObject element, bool value) => element.SetValue(IsReadOnlyProperty, value);

        public static bool GetIsReadOnly(DependencyObject element) => (bool)element.GetValue(IsReadOnlyProperty);

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.RegisterAttached("SelectedItems",
            typeof(IList), typeof(Element), new FrameworkPropertyMetadata(null, OnSelectedItemsChanged));

        public static IList GetSelectedItems(DependencyObject obj)
            => (IList)obj.GetValue(SelectedItemsProperty);

        public static void SetSelectedItems(DependencyObject obj, IList value)
            => obj.SetValue(SelectedItemsProperty, value);

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGrid dataGrid)
            {
                dataGrid.SelectionChanged -= DataGrid_SelectionChanged;

                if (e.NewValue != null)
                {
                    dataGrid.SelectionChanged += DataGrid_SelectionChanged;
                    dataGrid.SelectedItems.Clear();

                    if (e.NewValue is IList list)
                    {
                        foreach (var item in list)
                            dataGrid.SelectedItems.Add(item);
                    }
                }
            }
        }

        private static void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid)
            {
                var selectedItems = GetSelectedItems(dataGrid);
                if (selectedItems == null) return;

                // Đồng bộ từ DataGrid sang ViewModel
                foreach (var item in e.RemovedItems)
                    if (selectedItems.Contains(item))
                        selectedItems.Remove(item);

                foreach (var item in e.AddedItems)
                    if (!selectedItems.Contains(item))
                        selectedItems.Add(item);
            }
        }
    }
}
