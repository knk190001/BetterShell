using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BetterShell.Controls
{
    public partial class ApplicationBarIcon : UserControl
    {
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),typeof(ImageSource),typeof(ApplicationBarIcon));
        // public static DependencyProperty MultipleProperty = DependencyProperty.Register("Multiple",typeof(bool),typeof(ApplicationBarIcon));
        // public static DependencyProperty IsPrimaryProperty = DependencyProperty.Register("IsPrimary",typeof(bool),typeof(ApplicationBarIcon));

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(ApplicationBarIcon.IconProperty);
            set => SetValue(IconProperty, value);
        }

        public ApplicationBarIcon()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
}