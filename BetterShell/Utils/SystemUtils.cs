using System;
using System.Runtime.InteropServices;
using BetterShell.Controls;
using BetterShell.Utils.Win32Interop;

namespace BetterShell.Utils
{
    public static class SystemUtils
    {
        public static void SetWorkspace(RECT rect)
        {
            // Since you've declared the P/Invoke function correctly, you don't need to
            // do the marshaling yourself manually. The .NET FW will take care of it.
            
            
            var result = User32.SystemParametersInfo(User32.SPI_SETWORKAREA,
                0,
                ref rect ,
                0);
            if (!result)
            {
                throw new Exception($"Error setting desktop workspace: {Marshal.GetLastWin32Error()}");
            } 
        }
    }
}