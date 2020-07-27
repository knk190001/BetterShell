using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.System.Diagnostics;
using HWND = System.IntPtr;
using Size = Windows.Foundation.Size;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;


namespace BetterShell.Controls
{
    public partial class ApplicationBar : UserControl
    {
        public Applications Applications { get; set; }

        public ApplicationBar()
        {
            Applications = new Applications();
            InitializeComponent();
            
        }
    }


    public class Application
    {
        public Window Window { get; set; }
        public int Count { get; set; }
        public ImageSource Icon { get; set; }
        public string Identifier { get; set; }
    }

    public class Applications : ObservableCollection<Application>
    {
        public Applications()
        {
            var applications = OpenWindows();
            var iconsTask = GetIcons(applications);
            while (!iconsTask.IsCompleted)
            {
            }

            for (var i = 0; i < applications.Count; i++)
            {
                var id = GetIdentifier(applications[i]);

                Add(new Application()
                {
                    Count = 1,
                    Icon = iconsTask.Result[i],
                    Identifier = GetIdentifier(applications[i])
                });
            }
            
        }

        private static string GetIdentifier(Window application)
        {
            var id = application.process.ProcessName;
            try
            {
                if (application.process.MainModule != null)
                {
                    id = application.process.MainModule.FileName;
                }
            }
            catch
            {
                //ignored
            }

            return id;
        }

        private static async Task<BitmapSource[]> GetIcons(IEnumerable<Window> applications)
        {
            var icons = new List<BitmapSource>();

            foreach (var application in applications)
            {
                if (IsUwp(application))
                {
                    var bitmap = await GetUwpIcon(application);
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
                using (Icon ico = Icon.ExtractAssociatedIcon(application.process.MainModule.FileName))
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
                    new BitmapPalette(new List<System.Windows.Media.Color> {Colors.Transparent}),
                    new byte[] {0, 0, 0, 0},
                    1);
            }
        }

        private static async Task<BitmapImage> GetUwpIcon(Window application)
        {
            var uwpProcess = ProcessDiagnosticInfo.TryGetForProcessId((uint) application.process.Id);

            var randomAccessStreamReference =
                uwpProcess.GetAppDiagnosticInfos()[0].AppInfo.DisplayInfo.GetLogo(new Size(96, 96));
            var bitmap = new BitmapImage();

            using (var randomAccessStream = await randomAccessStreamReference.OpenReadAsync())
            {
                using (var stream = randomAccessStream.AsStream())
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }
            }

            return bitmap;
        }

        private static bool IsUwp(Window application)
        {
            return ProcessDiagnosticInfo.TryGetForProcessId((uint) application.process.Id).IsPackaged;
        }

        #region External

        // ReSharper disable IdentifierTypo


        //Used to get ID of any Window

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(WindowEnumProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);

        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);


        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowInfo(IntPtr hwnd, ref Windowinfo pwi);


        private delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
        private struct Windowinfo
        {
            // ReSharper disable IdentifierTypo
            public uint cbSize;
            public RECT rcWindow;
            public RECT rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;

            public ushort wCreatorVersion;
            // ReSharper restore IdentifierTypo

            public Windowinfo(bool? filler) :
                this() // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (uint) (Marshal.SizeOf(typeof(Windowinfo)));
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left, Top, Right, Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }

        // ReSharper restore IdentifierTypo


        private static int GetWindowProcessId(IntPtr hwnd)
        {
            int pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        private class Window
        {
            public Process process;
            public HWND hwnd;

            public Window(Process process, IntPtr hwnd)
            {
                this.process = process;
                this.hwnd = hwnd;
            }
        }

        private static List<Window> OpenWindows()
        {
            var result = new List<Window>();
            Process applicationFrameworkHost = null;
            EnumWindows(delegate(IntPtr hwnd, IntPtr lparam)
            {
                if (!IsWindowVisible(hwnd)) return true;

                var process = Process.GetProcessById(GetWindowProcessId(hwnd));
                if (process.ProcessName == "ApplicationFrameHost")
                {
                    applicationFrameworkHost = process;
                    return true;
                }

                Windowinfo info = new Windowinfo();

                GetWindowInfo(hwnd, ref info);

                var popup = 0x80000000;

                var toolWindow = 0x00000080;

                if ((info.dwExStyle & toolWindow) != 0 || (info.dwStyle & popup) != 0) return true;

                if (GetWindowTextLength(hwnd) == 0) return true;


                result.Add(new Window(process, hwnd));
                return true;
            }, 0);

            if (applicationFrameworkHost == null) return result;
            foreach (ProcessThread thread in applicationFrameworkHost.Threads)
            {
                EnumThreadWindows((uint) thread.Id, delegate(IntPtr hwnd, IntPtr lparam)
                {
                    EnumChildWindows(hwnd, delegate(IntPtr hwnd2, IntPtr lparam2)
                    {
                        var process = GetWindowProcessId(hwnd2);
                        if (process == applicationFrameworkHost.Id) return true;
                        result.Add(new Window(Process.GetProcessById(process), hwnd2));
                        return false;
                    }, IntPtr.Zero);
                    return false;
                }, IntPtr.Zero);
            }

            return result;
        }

        #endregion
    }
}