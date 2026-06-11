using Contellation.Custom.Helpers;

namespace Contellation.Custom.Structs
{
    /// <summary>
    /// Stores DPI information from which a <see cref="System.Windows.Media.Visual"/> or <see cref="System.Windows.UIElement"/>
    /// is rendered.
    /// </summary>
    public readonly struct DisplayDpi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayDpi"/> structure.
        /// </summary>
        /// <param Name="dpiScaleX">The DPI scale on the X axis.</param>
        /// <param Name="dpiScaleY">The DPI scale on the Y axis.</param>
        public DisplayDpi(double dpiScaleX, double dpiScaleY)
        {
            DpiScaleX = dpiScaleX;
            DpiScaleY = dpiScaleY;

            DpiX = (int)Math.Round(DpiHelper.DefaultDpi * dpiScaleX, MidpointRounding.AwayFromZero);
            DpiY = (int)Math.Round(DpiHelper.DefaultDpi * dpiScaleY, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayDpi"/> structure.
        /// </summary>
        /// <param Name="dpiX">The DPI on the X axis.</param>
        /// <param Name="dpiY">The DPI on the Y axis.</param>
        public DisplayDpi(int dpiX, int dpiY)
        {
            DpiX = dpiX;
            DpiY = dpiY;

            DpiScaleX = dpiX / (double)DpiHelper.DefaultDpi;
            DpiScaleY = dpiY / (double)DpiHelper.DefaultDpi;
        }

        /// <summary> Gets the DPI on the X axis. </summary>
        public int DpiX { get; }

        /// <summary> Gets the DPI on the Y axis. </summary>
        public int DpiY { get; }

        /// <summary> Gets the DPI scale on the X axis. </summary>
        public double DpiScaleX { get; }

        /// <summary> Gets the DPI scale on the Y axis. </summary>
        public double DpiScaleY { get; }
    }
}
