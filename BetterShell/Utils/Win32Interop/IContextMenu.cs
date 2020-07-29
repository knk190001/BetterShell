using System;
using System.Runtime.InteropServices;
using System.Text;
// ReSharper disable IdentifierTypo

namespace BetterShell.Utils.Win32Interop
{
    [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("000214e4-0000-0000-c000-000000000046")]
    public interface IContextMenu
    {
        [PreserveSig]
        HRESULT QueryContextMenu(IntPtr hmenu, int iMenu, int idCmdFirst, int idCmdLast, int uFlags);

        [PreserveSig]
        HRESULT InvokeCommand(ref CMINVOKECOMMANDINFO pici);

        [PreserveSig]
        HRESULT GetCommandString(int idCmd, int uFlags, IntPtr pwReserved,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMax);
    }
}