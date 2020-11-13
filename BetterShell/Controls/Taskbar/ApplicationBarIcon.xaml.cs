using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BetterShell.Utils;

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

        public static readonly DependencyProperty HWNDProperty = DependencyProperty.Register(
            nameof(HWND), typeof(IntPtr), typeof(ApplicationBarIcon), new PropertyMetadata(default(IntPtr)));

        public IntPtr HWND
        {
            get => (IntPtr) GetValue(HWNDProperty);
            set => SetValue(HWNDProperty, value);
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

        private void OnClick(object sender, RoutedEventArgs e)
        {
            RunningApplicationUtils.SetForegroundWindow(HWND);
        }
    }
}