using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using BetterShell.Controls;

namespace BetterShell.Utils.Win32Interop
{
    [StructLayout(LayoutKind.Sequential)]
    [SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Local")]
    public struct WINDOWINFO
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

        public WINDOWINFO(bool? filler) :
            this() // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
        {
            cbSize = (uint) (Marshal.SizeOf(typeof(WINDOWINFO)));
        }
    }
}