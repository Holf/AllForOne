using System.Diagnostics;
using System.Threading;

namespace Holf.AllForOne.Tests.AppWithManagedChildProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = false,
                CreateNoWindow = true,
                UseShellExecute = true,
                FileName = "ChromeDriver.exe"
            };

            var chromeDriverProcess = new Process { StartInfo = startInfo };

            chromeDriverProcess.Start();

            // The next statement ensures that when this program stops, the child process will stop too.
            chromeDriverProcess.TieLifecycleToParentProcess();

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
