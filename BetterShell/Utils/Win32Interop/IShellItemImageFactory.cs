using System;
using System.Runtime.InteropServices;
// ReSharper disable IdentifierTypo

namespace BetterShell.Utils.Win32Interop
{
    [ComImport]
    [GuidAttribute("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItemImageFactory
    {
        HRESULT GetImage(
            [In, MarshalAs(UnmanagedType.Struct)] SIZE size,
            [In] SIIGBF flags,
            [Out] out IntPtr phbm);
    }
}