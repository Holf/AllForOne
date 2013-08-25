using System;
using System.Runtime.InteropServices;

namespace Holf.AllForOne
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Security_Attributes
    {
        public UInt32 nLength;
        public IntPtr lpSecurityDescriptor;
        public Int32 bInheritHandle;
    }
}