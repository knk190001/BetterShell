using System;
using System.Runtime.InteropServices;
// ReSharper disable IdentifierTypo

namespace BetterShell.Utils.Win32Interop
{
    [ComImport]
    [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IShellItem
    {
        HRESULT BindToHandler(IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
            [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv);

        HRESULT GetParent(out IShellItem ppsi);
        HRESULT GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
        HRESULT GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
        HRESULT Compare(IShellItem psi, SICHINTF hint, out int piOrder);
    }
}