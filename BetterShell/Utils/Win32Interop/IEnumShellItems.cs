using System.Runtime.InteropServices;
// ReSharper disable IdentifierTypo

namespace BetterShell.Utils.Win32Interop
{
    [ComImport]
    [Guid("70629033-e363-4a28-a567-0db78006e6d7")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumShellItems
    {
        HRESULT Next(uint celt, out IShellItem rgelt, out uint pceltFetched);
        HRESULT Skip(uint celt);
        HRESULT Reset();
        HRESULT Clone(out IEnumShellItems ppenum);
    }
}