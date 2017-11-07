namespace Geeks.VSIX.Core.Utility
{
    public class ProcessActions
    {
        public static void GeeksProductivityToolsProcess(string processName)
        {
            var visualStudioProcesses = System.Diagnostics.Process.GetProcessesByName("devenv");
            foreach (var process in visualStudioProcesses)
            {
                if (!process.MainWindowTitle.Contains(processName)) continue;

                process.Kill();
                break;
            }
        }
    }
}
