using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BetterShell.StartMenu
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StartMenu
    {
        private Window _w;
        private bool _canHide = true;

        public StartMenu()
        {
            InitializeComponent();
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

        protected override void OnActivated(EventArgs e)
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
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
        }
    }
}