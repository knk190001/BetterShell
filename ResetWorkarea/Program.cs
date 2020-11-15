using System;
using BetterShell.Utils;
using BetterShell.Utils.Win32Interop;

namespace ResetWorkarea
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemUtils.SetWorkspace(new RECT() {Bottom = 1040, Left = 0, Right =1920, Top = 0});
        }
    }
}