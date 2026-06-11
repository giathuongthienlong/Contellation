using Contellation.Custom.Controls;
using System.Diagnostics.CodeAnalysis;
using System.Windows;

namespace Contellation.Custom.Events
{
    /// <summary>
    /// Event triggered on successful navigation.
    /// </summary>
    /// <param name="sender">Source of the event, which should be the current navigation instance.</param>
    /// <param name="e">Event data containing information about the navigation event.</param>
#if NET5_0_OR_GREATER
    public delegate void RoutedNotifyIconEvent([NotNull] NotifyIcon sender, RoutedEventArgs e);
#else
public delegate void RoutedNotifyIconEvent(NotifyIcon sender, RoutedEventArgs e);
#endif
}
