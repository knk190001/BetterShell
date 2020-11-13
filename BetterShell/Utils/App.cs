using System.Collections;
using System.Windows;

namespace BetterShell.Utils
{
    public enum AppType
    {
        None,
        Exe,
        AppX,
        Label
    }

    public class App: DependencyObject
    {
        public AppType Type;

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            nameof(Path), typeof(string), typeof(App), new PropertyMetadata(default(string)));

        public string Path
        {
            get;
            set;
        }
    }

    public class AppTree : Branch<App>,IEnumerable
    {
        public IEnumerator GetEnumerator()
        {
            return Children.GetEnumerator();
        }
    }
}