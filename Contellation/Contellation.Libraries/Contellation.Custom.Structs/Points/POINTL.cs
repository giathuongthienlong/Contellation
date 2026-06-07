using System.Runtime.InteropServices;

namespace Contellation.Custom.Structs.Points
{
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

    /// <summary>
    /// The <see cref="POINTL"/> structure defines the x- and y-coordinates of a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct POINTL
    {
        /// <summary>
        /// Specifies the x-coordinate of the point.
        /// </summary>
        public long x;

        /// <summary>
        /// Specifies the y-coordinate of the point.
        /// </summary>
        public long y;
    }

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
}
