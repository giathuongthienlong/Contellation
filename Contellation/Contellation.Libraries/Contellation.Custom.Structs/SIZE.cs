using System.Runtime.InteropServices;

namespace Contellation.Custom.Structs
{
    // ReSharper disable InconsistentNaming
#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

    /// <summary>
    /// The SIZE structure defines the width and height of a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        /// <summary>
        /// Specifies the rectangle's width. The units depend on which function uses this structure.
        /// </summary>
        public long cx;

        /// <summary>
        /// Specifies the rectangle's height. The units depend on which function uses this structure.
        /// </summary>
        public long cy;
    }

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
}
