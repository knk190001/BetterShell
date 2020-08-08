using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BetterShell.Utils.WinRTInterop;

namespace BetterShell.StartMenu
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StartMenu
    {
        private Window _w;
        private bool _canHide = true;
        private ApplicationActivationManager _applicationActivationManager;

        public StartMenu()
        {
            InitializeComponent();
            _applicationActivationManager = new ApplicationActivationManager();
            /*var w = new Window
            {
                Top = -100,
                Left = -100,
                Width = 1,
                Height = 1,
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false
            };
            
            w.Show(); 
            Owner = w; 
            w.Hide();
            Show();*/
        }

        /*protected override void OnActivated(EventArgs e)
        {
            if (_w == null)
            {
                _w = new Window
                {
                    Top = -100,
                    Left = -100,
                    Width = 1,
                    Height = 1,
                    WindowStyle = WindowStyle.ToolWindow,
                    ShowInTaskbar = false
                };
            }

            _w.Show();
            Owner = _w;
            _w.Hide();


            base.OnActivated(e);
        }*/

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
        }

        private void Office_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationActivationManager.ActivateApplication(
                "Microsoft.MicrosoftOfficeHub_8wekyb3d8bbwe!Microsoft.MicrosoftOfficeHub", null, ActivateOptions.None,
                out var pid);
        }

        private void OneDriver_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", @"shell:::{018D5C66-4533-4307-9B53-224DE2ED1FE6}");
        }
    }
}