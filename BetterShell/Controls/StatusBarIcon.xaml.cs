using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BetterShell.Controls
{
    public partial class StatusBarIcon : UserControl
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
            nameof(Icon), typeof(ImageSource), typeof(StatusBarIcon), new PropertyMetadata(default(ImageSource)));

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public static readonly DependencyProperty IconWidthProperty = DependencyProperty.Register(
            nameof(IconWidth), typeof(double), typeof(StatusBarIcon), new PropertyMetadata(default(double)));

        public double IconWidth
        {
            get => (double) GetValue(IconWidthProperty);
            set => SetValue(IconWidthProperty, value);
        }
        
        public StatusBarIcon()
        {
            InitializeComponent();
        }
    }
}