using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Windows.Management.Deployment;
using Windows.Web.AtomPub;
using BetterShell.Utils;
using BetterShell.Utils.Win32Interop;
using Application = System.Windows.Application;
using Point = System.Drawing.Point;

namespace BetterShell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ImageSource WallPaper
        {
            get;
        }


        public MainWindow()
        {
            InitializeComponent();

            
            var bg = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop","WallPaper",null);
            var uri = new Uri(bg.ToString(),UriKind.Absolute);

            WallPaper = new BitmapImage(uri);
            
            Background = new ImageBrush(WallPaper);
            
            var bounds = Screen.PrimaryScreen.Bounds;
            WindowState = WindowState.Normal;
            Height = bounds.Height;
            Width = bounds.Width;
            Left = bounds.Left;
            Top = bounds.Top;
            // WindowState = WindowState.Maximized;
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var bounds = Screen.PrimaryScreen.Bounds;
            WindowState = WindowState.Normal;
            Left = bounds.Left;
            Top = bounds.Top;
            Width = bounds.Width;
            Height = bounds.Height;
            WindowState = WindowState.Maximized;
            var workingArea = Screen.PrimaryScreen.Bounds;
            SystemUtils.SetWorkspace(new RECT() {Bottom = workingArea.Bottom-60, Left = workingArea.Left, Right = workingArea.Right, Top = workingArea.Top});
        }
    }
}
