using System;
using System.Runtime.InteropServices;
// ReSharper disable IdentifierTypo

namespace BetterShell.Utils.Win32Interop
{
    public static class Shell32
    {
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern HRESULT SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath,
            out IntPtr ppIdl, ref uint rgflnOut);
        
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern HRESULT SHCreateItemFromIDList(IntPtr pidl, [In, MarshalAs(UnmanagedType.LPStruct)]
            Guid riid, out IShellItem ppv);
    }
}