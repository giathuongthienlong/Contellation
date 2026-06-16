using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Contellation.Custom.Controls.Datas.DataGrids.Generic;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// ScrollBar to Top
    /// https://itecnote.com/tecnote/r-wpf-reset-listbox-scroll-position-when-itemssource-changes/
    /// </summary>
    public static class ScrollToTopBehavior
    {
        public static readonly DependencyProperty ScrollToTopProperty = DependencyProperty.RegisterAttached("ScrollToTop",
            typeof(bool), typeof(ScrollToTopBehavior), new UIPropertyMetadata(false, OnScrollToTopPropertyChanged));

        public static bool GetScrollToTop(DependencyObject obj)
        {
            return (bool)obj.GetValue(ScrollToTopProperty);
        }

        public static void SetScrollToTop(DependencyObject obj, bool value)
        {
            obj.SetValue(ScrollToTopProperty, value);
        }

        private static void ItemsSourceChanged(object o, EventArgs eArgs)
        {
            ItemsControl itemsControl = o as ItemsControl;

            if (itemsControl == null) return;

            void EventHandler(object sender, EventArgs e)
            {
                if (itemsControl.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
                {
                    ScrollViewer scrollViewer = itemsControl.FindVisualChild<ScrollViewer>();
                    scrollViewer.ScrollToTop();
                    itemsControl.ItemContainerGenerator.StatusChanged -= EventHandler;
                }
            }

            itemsControl.ItemContainerGenerator.StatusChanged += EventHandler;
        }

        private static void OnScrollToTopPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            ItemsControl itemsControl = dpo as ItemsControl;

            if (itemsControl != null)
            {
                DependencyPropertyDescriptor dependencyPropertyDescriptor =
                    DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty,
                        typeof(ItemsControl));

                if (dependencyPropertyDescriptor != null)
                {
                    if ((bool)e.NewValue)
                    {
                        dependencyPropertyDescriptor.AddValueChanged(itemsControl, ItemsSourceChanged);
                    }
                    else
                    {
                        dependencyPropertyDescriptor.RemoveValueChanged(itemsControl, ItemsSourceChanged);
                    }
                }
            }
        }
    }
}
