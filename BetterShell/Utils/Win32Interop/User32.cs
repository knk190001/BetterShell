using System;
using System.Runtime.InteropServices;
using BetterShell.Controls;

namespace BetterShell.Utils.Win32Interop
{
    public static class User32
    {
        //Used to get ID of any Window

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

        [DllImport("USER32.DLL")]
        public static extern bool EnumWindows(WindowEnumProc enumFunc, int lParam);

        [DllImport("USER32.DLL")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, out string lpString, int nMaxCount);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);


        public delegate bool EnumThreadDelegate(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
        
        [DllImport("User32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        internal static extern bool SendMessage(IntPtr hWnd, Int32 msg, Int32 wParam, Int32 lParam);
        public static readonly int WM_SYSCOMMAND = 0x0112;
        public static readonly int SC_RESTORE = 0xF120;
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(
            int uiAction,
            int uiParam,
            ref RECT pvParam,
            int fWinIni);

        public static readonly int SPIF_SENDWININICHANGE = 2;
        public static readonly int SPIF_UPDATEINIFILE = 1;
        public static readonly int SPIF_change = SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE;
        public static readonly int SPI_SETWORKAREA = 47;
        public static readonly int SPI_GETWORKAREA = 48;
    }
}