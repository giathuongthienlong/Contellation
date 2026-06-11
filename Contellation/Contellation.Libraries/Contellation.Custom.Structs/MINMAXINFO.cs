using Contellation.Custom.Structs.Points;
using System.Runtime.InteropServices;

namespace Contellation.Custom.Structs
{
    [StructLayout(LayoutKind.Sequential)]
    public class MINMAXINFO
    {
        public POINT ptReserved;
        public POINT ptMaxSize;
        public POINT ptMaxPosition;
        public POINT ptMinTrackSize;
        public POINT ptMaxTrackSize;
    }
}
