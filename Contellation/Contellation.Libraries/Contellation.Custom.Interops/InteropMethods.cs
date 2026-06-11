using Contellation.Custom.Properties;
using Contellation.Custom.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using static Contellation.Custom.Interops.User32;

namespace Contellation.Custom.Interops
{
    internal class InteropMethods
    {
        #region common

        //-

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.User32, SetLastError = true, ExactSpelling = true, EntryPoint = nameof(GetDC), CharSet = CharSet.Auto)]
        internal static extern IntPtr IntGetDC(HandleRef hWnd);
        [SecurityCritical]
        internal static IntPtr GetDC(HandleRef hWnd)
        {
            var hDc = IntGetDC(hWnd);
            if (hDc == IntPtr.Zero) throw new Win32Exception();

            return Collector.Add(hDc, Common.HDC);
        }

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.User32, ExactSpelling = true, EntryPoint = nameof(ReleaseDC), CharSet = CharSet.Auto)]
        internal static extern int IntReleaseDC(HandleRef hWnd, HandleRef hDC);

        [SecurityCritical]
        internal static int ReleaseDC(HandleRef hWnd, HandleRef hDC)
        {
            Collector.Remove((IntPtr)hDC, Common.HDC);
            return IntReleaseDC(hWnd, hDC);
        }

        [DllImport(LibrariesDLL.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO monitorInfo);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(HandleRef hDC, int nIndex);

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(LibrariesDLL.User32)]
        internal static extern int GetSystemMetrics(SM nIndex);

        [DllImport(LibrariesDLL.User32, SetLastError = true)]
        internal static extern int ReleaseDC(IntPtr window, IntPtr dc);

        [DllImport(LibrariesDLL.Gdi32, SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(LibrariesDLL.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetDC(IntPtr ptr);


        [DllImport(LibrariesDLL.Shell32, CallingConvention = CallingConvention.StdCall)]
        internal static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

        [DllImport(LibrariesDLL.User32)]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion


        [DllImport(LibrariesDLL.User32)]
        internal static extern IntPtr GetActiveWindow();


        [DllImport(LibrariesDLL.User32)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        internal static int GetWindowLong(IntPtr hWnd, User32.GWL nIndex) => GetWindowLong(hWnd, (int)nIndex);

        internal static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong) => IntPtr.Size == 4
            ? SetWindowLongPtr32(hWnd, nIndex, dwNewLong)
            : SetWindowLongPtr64(hWnd, nIndex, dwNewLong);


        [DllImport(LibrariesDLL.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLong")]
        internal static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(LibrariesDLL.User32, CharSet = CharSet.Auto, EntryPoint = "SetWindowLongPtr")]
        internal static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport(LibrariesDLL.User32, CharSet = CharSet.Unicode)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(LibrariesDLL.User32, CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        internal static IntPtr SetWindowLongPtr(IntPtr hWnd, User32.GWLP nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
            {
                return SetWindowLongPtr(hWnd, (int)nIndex, dwNewLong);
            }
            return new IntPtr(SetWindowLong(hWnd, (int)nIndex, dwNewLong.ToInt32()));
        }

        internal static int SetWindowLong(IntPtr hWnd, User32.GWL nIndex, int dwNewLong) => SetWindowLong(hWnd, (int)nIndex, dwNewLong);

    }
}
