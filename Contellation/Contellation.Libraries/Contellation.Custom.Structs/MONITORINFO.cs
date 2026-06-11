using Contellation.Custom.Structs.Rects;

namespace Contellation.Custom.Structs
{
    public struct MONITORINFO
    {
        public uint cbSize;
        public RECT rcMonitor;
        public RECT rcWork;
        public uint dwFlags;
    }
}
