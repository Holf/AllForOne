using System.Diagnostics;

namespace Holf.AllForOne
{
    public static class ProcessExtensions
    {
        public static void TieLifecycleToParentProcess(this Process process)
        {
            LifecycleManagedProcessList.AddProcess(process);
        }
    }
}
