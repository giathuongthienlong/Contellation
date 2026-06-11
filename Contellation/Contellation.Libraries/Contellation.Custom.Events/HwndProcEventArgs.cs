namespace Contellation.Custom.Events
{
    public class HwndProcEventArgs : EventArgs
    {
        public bool Handled { get; set; }

        public IntPtr? ReturnValue { get; set; }

        public bool IsMouseOverDetectedHeaderContent { get; }

        public IntPtr HWND { get; }

        public int Message { get; }

        public IntPtr WParam { get; }

        public IntPtr LParam { get; }

        internal HwndProcEventArgs(
            IntPtr hwnd,
            int msg,
            IntPtr wParam,
            IntPtr lParam,
            bool isMouseOverDetectedHeaderContent
        )
        {
            HWND = hwnd;
            Message = msg;
            WParam = wParam;
            LParam = lParam;
            IsMouseOverDetectedHeaderContent = isMouseOverDetectedHeaderContent;
        }
    }
}
