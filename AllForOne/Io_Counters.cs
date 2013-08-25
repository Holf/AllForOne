using System;
using System.Runtime.InteropServices;

namespace Holf.AllForOne
{
    [StructLayout(LayoutKind.Sequential)]
    struct Io_Counters
    {
        public UInt64 ReadOperationCount;
        public UInt64 WriteOperationCount;
        public UInt64 OtherOperationCount;
        public UInt64 ReadTransferCount;
        public UInt64 WriteTransferCount;
        public UInt64 OtherTransferCount;
    }
}