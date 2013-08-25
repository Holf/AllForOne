using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Holf.AllForOne
{
    /// <summary>
    /// Please see here for further info: 
    /// http://stackoverflow.com/questions/6266820/working-example-of-createjobobject-setinformationjobobject-pinvoke-in-net/9164742#9164742
    /// The code below is different in two respects:
    /// - It's all static and I'm using a destructor to kill the handles rather than doing this in an IDisposable implementation.
    /// - The 'LimitFlags' setting is '0x3000' rather than '0x2000'. This applies 'JOB_OBJECT_LIMIT_SILENT_BREAKAWAY_OK' (as well as
    ///   'JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE'). Without it, any child processes spawned by ChromeDriver, such as instances of
    ///   Chrome, get assigned and locked to the same Job. Chrome cannot thereafter use it's own Job Management strategies, and
    ///   therefore crashes. See here for more info:
    ///   http://msdn.microsoft.com/en-us/library/windows/desktop/ms684161(v=vs.85).aspx
    ///   http://msdn.microsoft.com/en-us/library/windows/desktop/ms684147(v=vs.85).aspx
    /// </summary>
    public class LifecycleManagedProcessList
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr CreateJobObject(IntPtr a, string lpName);

        [DllImport("kernel32.dll")]
        static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, IntPtr lpJobObjectInfo, UInt32 cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        private static IntPtr handle;
        //private static bool disposed;

        static LifecycleManagedProcessList()
        {
            handle = CreateJobObject(IntPtr.Zero, null);

            var info = new JobObject_Basic_Limit_Information
                {
                    LimitFlags = 0x3000
                };

            var extendedInfo = new JobObject_Extended_Limit_Information
                {
                    BasicLimitInformation = info,

                };

            int length = Marshal.SizeOf(typeof(JobObject_Extended_Limit_Information));
            IntPtr extendedInfoPtr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(extendedInfo, extendedInfoPtr, false);

            if (!SetInformationJobObject(handle, JobObjectInfoType.ExtendedLimitInformation, extendedInfoPtr, (uint)length))
                throw new Exception(string.Format("Unable to set information.  Error: {0}", Marshal.GetLastWin32Error()));
        }

        ~LifecycleManagedProcessList()
        {
            KillChildProcesses();
        }

        private static void KillChildProcesses()
        {
            CloseHandle(handle);
            handle = IntPtr.Zero;
        }

        public static bool AddProcess(Process process)
        {
            return AssignProcessToJobObject(handle, process.Handle);
        }
    }
}