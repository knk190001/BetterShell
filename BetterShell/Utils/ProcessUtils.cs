using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.System.Diagnostics;

namespace BetterShell.Utils
{
    public static class ProcessUtils
    {
        public static readonly IEnumerable<Process> Processes = Process.GetProcesses().ToList();

        public static readonly IReadOnlyList<ProcessDiagnosticInfo> ProcessDiagnosticInfos =
            ProcessDiagnosticInfo.GetForProcesses();
    }
}