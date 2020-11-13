using System;
using System.Diagnostics;

namespace BetterShell.Utils
{
    public class Window
    {
        public Process process;
        public IntPtr hwnd;

        public Window(Process process, IntPtr hwnd)
        {
            this.process = process;
            this.hwnd = hwnd;
        }
    }
}