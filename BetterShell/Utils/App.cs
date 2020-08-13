namespace BetterShell.Utils
{
    public enum AppType
    {
        None,
        Exe,
        AppX
    }

    public struct App
    {
        public string Path;
        public AppType Type;
    }
}