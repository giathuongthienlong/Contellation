using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Contellation.Custom.Interops
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Common
    {
        public static readonly int HDC = Collector.RegisterType(nameof(HDC), 100, 2);

        public static readonly int GDI = Collector.RegisterType(nameof(GDI), 50, 500);

        public static readonly int Kernel = Collector.RegisterType(nameof(Kernel), 0, 1000);
    }
}
