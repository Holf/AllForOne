using System.Diagnostics;
using System.Linq;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;

namespace Holf.AllForOne.Tests
{
    [TestFixture]
    public class TestFixture
    {
        private const string ChildAppName = "chromedriver";

        [Test]
        public void Killing_an_App_with_a_Managed_Child_Process_should_also_kill_the_Child_Process()
        {
            // Arrange
            var appWithManagedChildProcess = StartApp("AppWithManagedChildProcess.exe");

            // Act
            appWithManagedChildProcess.Kill();

            // Assert
            var childChromeDriverProcesses = Process.GetProcessesByName(ChildAppName);
            childChromeDriverProcesses.Should().BeEmpty();
        }

        [Test]
        public void Killing_an_App_with_an_Unmanaged_Child_Process_should_leave_the_Child_Process_running()
        {
            // Arrange
            var appWithUnmanagedChildProcess = StartApp("AppWithUnmanagedChildProcess.exe");

            // Act
            appWithUnmanagedChildProcess.Kill();

            // Assert
            var childChromeDriverProcesses = Process.GetProcessesByName(ChildAppName);
            childChromeDriverProcesses.Should().HaveCount(1);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up any hanging child ChromeDriver processes.
            var chromeDriverProcesses = Process.GetProcessesByName(ChildAppName);
            chromeDriverProcesses.ToList().ForEach(x => x.Kill());
        }

        private Process StartApp(string fileName)
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Normal,
                ErrorDialog = false,
                CreateNoWindow = false,
                UseShellExecute = true,
                FileName = fileName
            };

            var app = new Process { StartInfo = startInfo };
            app.Start();

            // Pause to ensure both App & Child App processes are running.
            Thread.Sleep(2000);

            return app;
        }
    }
}
