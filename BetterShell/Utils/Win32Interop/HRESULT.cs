// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
namespace BetterShell.Utils.Win32Interop
{
    public enum HRESULT
    {
        S_OK = 0,
        S_FALSE = 1,
        E_NOINTERFACE = unchecked((int) 0x80004002),
        E_NOTIMPL = unchecked((int) 0x80004001),
        E_FAIL = unchecked((int) 0x80004005)
    }
}