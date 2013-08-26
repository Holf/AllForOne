using System.Diagnostics;
using System.Threading;

namespace Holf.AllForOne.Tests.AppWithUnmanagedChildProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = false,
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = "testChromedriver.exe"
            };

            var chromeDriverProcess = new Process { StartInfo = startInfo };

            chromeDriverProcess.Start();

            // Even if this program is stopped, the child process will continue.

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
