using Contellation.Custom.Controls;
using System.Windows;

namespace Contellation.Custom.Interfaces.Control.Dragablz
{
    public interface INewTabHost<out TElement> where TElement : UIElement
    {
        TElement Container { get; }
        TabablzControl TabablzControl { get; }
    }
}
