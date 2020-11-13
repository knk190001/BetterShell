using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BetterShell.Utils;

namespace BetterShell.StartMenu.Controls
{
    public partial class Application : UserControl
    {
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(Application), new PropertyMetadata(default(string)));

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(ImageSource), typeof(Application), new PropertyMetadata(default(ImageSource)));
    
        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type), typeof(AppType), typeof(Application), new PropertyMetadata(default(AppType)));

        public AppType Type
        {
            get => (AppType) GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            nameof(Path), typeof(string), typeof(Application), new PropertyMetadata(default(string)));

        public string Path
        {
            get => (string) GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        
        public Application()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ApplicationExecutor.Instance.Run(Path,Type);
        }
    }
}