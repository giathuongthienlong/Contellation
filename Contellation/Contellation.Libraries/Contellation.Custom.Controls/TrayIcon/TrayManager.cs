using Contellation.Custom.Handlers.Control;
using Contellation.Custom.Interfaces.Control;
using Contellation.Custom.Interops;
using System.Windows;
using System.Windows.Interop;

namespace Contellation.Custom.Controls
{
    /// <summary>
    /// Responsible for managing the icons in the Tray bar.
    /// </summary>
    internal static class TrayManager
    {
        public static bool Register(INotifyIcon notifyIcon)
        {
            if (notifyIcon is null)
            {
                return false;
            }

            return Register(notifyIcon, GetParentSource());
        }

        public static bool Register(INotifyIcon notifyIcon, System.Windows.Window parentWindow)
        {
            if (parentWindow == null)
            {
                return false;
            }

            return Register(notifyIcon, (HwndSource)PresentationSource.FromVisual(parentWindow));
        }

        public static bool Register(INotifyIcon notifyIcon, HwndSource? parentSource)
        {
            if (parentSource is null)
            {
                if (!notifyIcon.IsRegistered)
                {
                    return false;
                }

                _ = Unregister(notifyIcon);

                return false;
            }

            if (parentSource.Handle == IntPtr.Zero)
            {
                return false;
            }

            if (notifyIcon.IsRegistered)
            {
                _ = Unregister(notifyIcon);
            }

            notifyIcon.Id = TrayData.NotifyIcons.Count + 1;

            notifyIcon.HookWindow = new TrayHandler(
                $"wpfui_th_{parentSource.Handle}_{notifyIcon.Id}",
                parentSource.Handle
            )
            {
                ElementId = notifyIcon.Id
            };

            notifyIcon.ShellIconData = new Shell32.NOTIFYICONDATA
            {
                uID = notifyIcon.Id,
                uFlags = Shell32.NIF.MESSAGE,
                uCallbackMessage = (int)User32.WM.TRAYMOUSEMESSAGE,
                hWnd = notifyIcon.HookWindow.Handle,
                dwState = 0x2
            };

            if (!string.IsNullOrEmpty(notifyIcon.TooltipText))
            {
                notifyIcon.ShellIconData.szTip = notifyIcon.TooltipText;
                notifyIcon.ShellIconData.uFlags |= Shell32.NIF.TIP;
            }

            ReloadHicon(notifyIcon);

            notifyIcon.HookWindow.AddHook(notifyIcon.WndProc);

            _ = Shell32.Shell_NotifyIcon(Shell32.NIM.ADD, notifyIcon.ShellIconData);

            TrayData.NotifyIcons.Add(notifyIcon);

            notifyIcon.IsRegistered = true;

            return true;
        }

        public static bool ModifyIcon(INotifyIcon notifyIcon)
        {
            if (!notifyIcon.IsRegistered)
            {
                return true;
            }

            ReloadHicon(notifyIcon);

            return Shell32.Shell_NotifyIcon(Shell32.NIM.MODIFY, notifyIcon.ShellIconData);
        }

        /// <summary>
        /// Tries to remove the <see cref="INotifyIcon"/> from the shell.
        /// </summary>
        public static bool Unregister(INotifyIcon notifyIcon)
        {
            if (notifyIcon.ShellIconData == null || !notifyIcon.IsRegistered)
            {
                return false;
            }

            _ = Shell32.Shell_NotifyIcon(Shell32.NIM.DELETE, notifyIcon.ShellIconData);

            notifyIcon.IsRegistered = false;

            return true;
        }

        /// <summary>
        /// Gets application source.
        /// </summary>
        private static HwndSource? GetParentSource()
        {
            Window mainWindow = (Window)Application.Current.MainWindow;

            if (mainWindow == null)
            {
                return null;
            }

            return (HwndSource)PresentationSource.FromVisual(mainWindow);
        }

        private static void ReloadHicon(INotifyIcon notifyIcon)
        {
            IntPtr hIcon = IntPtr.Zero;

            if (notifyIcon.Icon is not null)
            {
                hIcon = Hicon.FromSource(notifyIcon.Icon);
            }

            if (hIcon == IntPtr.Zero)
            {
                hIcon = Hicon.FromApp();
            }

            if (hIcon != IntPtr.Zero)
            {
                notifyIcon.ShellIconData.hIcon = hIcon;
                notifyIcon.ShellIconData.uFlags |= Shell32.NIF.ICON;
            }
        }
    }
}
