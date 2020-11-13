using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.System;
using Windows.System.Diagnostics;
using BetterShell.Controls;
using BetterShell.Utils.Win32Interop;

namespace BetterShell.Utils
{
    public static class RunningApplicationUtils
    {
        public static string GetIdentifier(Window application)
        {
            var id = "";
            if (IsUwp(application))
            {
                id = GetUwpId(application);
            }
            else
            {
                try
                {
                    if (application.process.MainModule != null)
                    {
                        var mainModule = application.process.MainModule;
                        id = mainModule.FileName.Substring(0,
                                 mainModule.FileName.Length - mainModule.ModuleName.Length) +
                             mainModule.ModuleName.ToLower();
                    }
                }
                catch
                {
                    //ignored
                }
            }

            return id;
        }

        public static bool IsUwp(Window application)
        {
            return ProcessUtils.ProcessDiagnosticInfos.First(info => info.ProcessId == application.process.Id)
                .IsPackaged;
        }

        private static int GetWindowProcessId(IntPtr hwnd)
        {
            int pid;
            User32.GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }


        public static List<Window> OpenWindows()
        {
            var result = new List<Window>();
            Process applicationFrameworkHost = null;
            User32.EnumWindows(delegate(IntPtr hwnd, IntPtr lparam)
            {
                var process = ProcessUtils.Processes.First(process1 => process1.Id == GetWindowProcessId(hwnd));

                if (!User32.IsWindowVisible(hwnd)) return true;

                if (process.ProcessName == "ApplicationFrameHost")
                {
                    applicationFrameworkHost = process;
                    return true;
                }

                var info = new WINDOWINFO();

                User32.GetWindowInfo(hwnd, ref info);

                const int popup = 0x00000000;

                const int toolWindow = 0x00000080;

                const int noActivate = 0x08000000;

                const int noRedirectionBitmap = 0x00200000;

                if ((info.dwExStyle & (toolWindow | noActivate | noRedirectionBitmap)) != 0 ||
                    (info.dwStyle & (popup)) != 0) return true;

                if (User32.GetWindowTextLength(hwnd) == 0) return true;

                result.Add(new Window(process, hwnd));
                return true;
            }, 0);

            if (applicationFrameworkHost == null) return result;

            var tasks = applicationFrameworkHost.Threads
                .Cast<ProcessThread>()
                .Select(thread => FindUwpWindows(thread, applicationFrameworkHost))
                .ToArray();

            Task.WaitAll(tasks.Cast<Task>().ToArray());
            return result.Where(NotSuspended).ToList();
        }

        private static Task<List<Window>> FindUwpWindows(ProcessThread thread, Process applicationFrameworkHost)
        {
            var result = Task.Run(() =>
            {
                var windows = new List<Window>();

                User32.EnumThreadWindows((uint) thread.Id, delegate(IntPtr hwnd, IntPtr lparam)
                {
                    User32.EnumChildWindows(hwnd, delegate(IntPtr hwnd2, IntPtr lparam2)
                    {
                        var process = GetWindowProcessId(hwnd2);

                        if (process == applicationFrameworkHost.Id) return true;
                        windows.Add(new Window(Process.GetProcessById(process), hwnd2));
                        return false;
                    }, IntPtr.Zero);
                    return false;
                }, IntPtr.Zero);

                return windows;
            });
            return result;
        }
        private static bool NotSuspended(Window window)
        {
            var diagnosticInfo = ProcessDiagnosticInfo.TryGetForProcessId((uint) window.process.Id);
            if (!diagnosticInfo.IsPackaged)
            {
                return true;
            }

            var appDiagnosticInfos = diagnosticInfo.GetAppDiagnosticInfos();
            var anySuspended = appDiagnosticInfos.Any(info =>
                info.GetResourceGroups().Any(resourceInfo =>
                    resourceInfo.GetStateReport().ExecutionState == AppResourceGroupExecutionState.Suspended
                )
            );

            return !anySuspended;
        }
        
        private static readonly IShellItem[] InstalledApplications = ApplicationUtils.GetApplications();

        private static ImageSource GetUwpIcon(Window application)
        {
            var uwpProcess = ProcessDiagnosticInfo.TryGetForProcessId((uint) application.process.Id);
            
            var appInfos = uwpProcess.GetAppDiagnosticInfos();
            var appInfoCount = appInfos.Count;
            var appInfo = appInfos[0].AppInfo;
            var name = appInfo.AppUserModelId;


            var shellItem =
                InstalledApplications.First(item => ApplicationUtils.GetAppModelUserId(item) == appInfo.AppUserModelId);
            var bitmap = ApplicationUtils.GetIcon(shellItem);


            return bitmap;
        }
        
        private static string GetUwpId(Window application)
        {
            var process = ProcessDiagnosticInfo.TryGetForProcessId((uint) application.process.Id);
            foreach (var appDiagnosticInfo in process.GetAppDiagnosticInfos())
            {
                return appDiagnosticInfo.AppInfo.AppUserModelId;
            }

            return "None";
        }

        public static ImageSource[] GetIcons(IEnumerable<Window> applications)
        {
            var icons = new List<ImageSource>();

            foreach (var application in applications)
            {
                if (RunningApplicationUtils.IsUwp(application))
                {
                    var bitmap = GetUwpIcon(application);
                    if (bitmap == null)
                    {
                        Console.WriteLine(GetUwpId(application));

                        icons.Add(BitmapSource.Create(
                            2,
                            2,
                            96,
                            96,
                            PixelFormats.Indexed1,
                            new BitmapPalette(new List<System.Windows.Media.Color> {Colors.Red}),
                            new byte[] {0, 0, 0, 0},
                            1));
                        continue;
                    }

                    icons.Add(bitmap);
                }
                else
                {
                    icons.Add(GetIcon(application));
                }
            }

            return icons.ToArray();
        }
        private static BitmapSource GetIcon(Window application)
        {
            try
            {
                using (var ico = Icon.ExtractAssociatedIcon(application.process.MainModule.FileName))
                {
                    return Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch (Exception e)
            {
                return BitmapSource.Create(
                    2,
                    2,
                    96,
                    96,
                    PixelFormats.Indexed1,
                    new BitmapPalette(new List<System.Windows.Media.Color> {Colors.Blue}),
                    new byte[] {0, 0, 0, 0},
                    1);
            }
        }
    }
}