using Contellation.Custom.Controls;
using System.Windows;

namespace Contellation.Custom.Handlers.Control.Dragablz
{
    public delegate void FloatRequestedEventHandler(object sender, FloatRequestedEventArgs e);

    public class FloatRequestedEventArgs : DragablzItemEventArgs
    {
        public FloatRequestedEventArgs(RoutedEvent routedEvent, object source, DragablzItem dragablzItem)
            : base(routedEvent, source, dragablzItem)
        { }

        public FloatRequestedEventArgs(RoutedEvent routedEvent, DragablzItem dragablzItem)
            : base(routedEvent, dragablzItem)
        { }
    }
}
