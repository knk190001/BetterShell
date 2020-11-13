using System;
using System.Diagnostics;
using BetterShell.Utils.WinRTInterop;

namespace BetterShell.Utils
{
    public class ApplicationExecutor
    {
        public static ApplicationExecutor Instance { get; } = new ApplicationExecutor();
        private static ApplicationActivationManager ActivationManager { get; } = new ApplicationActivationManager();

        public void Run(string path, AppType type)
        {
            switch (type)
            {
                case AppType.None:
                    return;
                case AppType.Exe:
                    RunExe(path);
                    break;
                case AppType.AppX:
                    RunAppx(path);
                    break;
                case AppType.Label:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void RunAppx(string path)
        {
            ActivationManager.ActivateApplication(path, "", ActivateOptions.None, out var pid);
        }

        private static void RunExe(string path)
        {
            Process.Start(path);
            
        }
    }
}