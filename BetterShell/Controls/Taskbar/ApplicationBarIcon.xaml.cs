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

        public static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register(
            nameof(ToolTipText), typeof(string), typeof(ApplicationBarIcon), new PropertyMetadata(default(string)));

        public string ToolTipText
        {
            get => (string) GetValue(ToolTipTextProperty);
            set => SetValue(ToolTipTextProperty, value);
        }
        
        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public ApplicationBarIcon()
        {
            InitializeComponent();
        }
    }
}