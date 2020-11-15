using System;
using System.Runtime.InteropServices;

namespace BetterShell.Utils.Win32Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }
}