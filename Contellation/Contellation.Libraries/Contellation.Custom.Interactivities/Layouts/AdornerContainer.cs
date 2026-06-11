using System.Windows;
using System.Windows.Media;

namespace Contellation.Custom.Interactivities.Layouts
{
    public class AdornerContainer : System.Windows.Documents.Adorner
    {
        private UIElement child;

        public AdornerContainer(UIElement adornedElement) : base(adornedElement) { }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.child != null) { this.child.Arrange(new Rect(finalSize)); }

            return finalSize;
        }

        public UIElement Child
        {
            get { return this.child; }
            set
            {
                this.AddVisualChild(value);
                this.child = value;
            }
        }

        protected override int VisualChildrenCount { get { return this.child == null ? 0 : 1; } }

        protected override Visual GetVisualChild(int index)
        {
            return index == 0 && this.child != null ? this.child : base.GetVisualChild(index);
        }
    }
}
