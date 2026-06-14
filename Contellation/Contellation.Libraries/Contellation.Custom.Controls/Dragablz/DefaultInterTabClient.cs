using Contellation.Custom.Controls.Dragablz.Core;
using Contellation.Custom.Enums.Control.Dragablz;
using Contellation.Custom.Interfaces.Control;
using Contellation.Custom.Interfaces.Control.Dragablz;
using System.Windows.Threading;

namespace Contellation.Custom.Controls
{
    public class DefaultInterTabClient : IInterTabClient
    {
        public virtual INewTabHost<System.Windows.Window> GetNewHost(IInterTabClient interTabClient, object partition, TabablzControl source)
        {
            if (source == null) throw new ArgumentNullException("source");
            var sourceWindow = System.Windows.Window.GetWindow(source);
            if (sourceWindow == null) throw new ApplicationException("Unable to ascertain source window.");
            var newWindow = (System.Windows.Window)Activator.CreateInstance(sourceWindow.GetType());

            newWindow.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.DataBind);

            var newTabablzControl = newWindow.LogicalTreeDepthFirstTraversal().OfType<TabablzControl>().FirstOrDefault();
            if (newTabablzControl == null) throw new ApplicationException("Unable to ascertain tab control.");

            if (newTabablzControl.ItemsSource == null)
                newTabablzControl.Items.Clear();

            return new NewTabHost<System.Windows.Window>(newWindow, newTabablzControl);
        }

        public virtual TabEmptiedResponse TabEmptiedHandler(TabablzControl tabControl, System.Windows.Window window)
        {
            return TabEmptiedResponse.CloseWindowOrLayoutBranch;
        }
    }
}
