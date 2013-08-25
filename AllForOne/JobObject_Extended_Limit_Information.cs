using System;
using System.Runtime.InteropServices;

namespace Holf.AllForOne
{
    [StructLayout(LayoutKind.Sequential)]
    struct JobObject_Extended_Limit_Information
    {
        public JobObject_Basic_Limit_Information BasicLimitInformation;
        public Io_Counters IoInfo;
        public UIntPtr ProcessMemoryLimit;
        public UIntPtr JobMemoryLimit;
        public UIntPtr PeakProcessMemoryUsed;
        public UIntPtr PeakJobMemoryUsed;
    }
}