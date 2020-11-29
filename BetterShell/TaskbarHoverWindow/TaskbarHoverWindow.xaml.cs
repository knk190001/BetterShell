using System;
using System.Windows;
using System.Windows.Threading;
using BetterShell.Utils;
using Window = System.Windows.Window;

namespace BetterShell.TaskbarHoverWindow
{
    public partial class TaskbarHoverWindow : Window
    {
        public static TaskbarHoverWindow Instance { get; } = new TaskbarHoverWindow();

        private DispatcherTimer _hoverTimer = new DispatcherTimer();

        private Point targetLocation;
        private HWNDList _hwnds;

        public TaskbarHoverWindow()
        {
            InitializeComponent();
            _hoverTimer.Interval = TimeSpan.FromSeconds(.75);
            _hoverTimer.Tick += FirstHoverComplete;

            Owner = null;
        }

        public void StartHover(Point point, HWNDList hwnds)
        {
            targetLocation = point;
            _hoverTimer.Start();
            _hwnds = hwnds;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            Hide();
        }

        private void FirstHoverComplete(object sender, EventArgs eventArgs)
        {
            _hoverTimer.Stop();
            _hoverTimer.Interval = TimeSpan.FromSeconds(.75);
            //Activate();
            Focus();
            Show();
            Left = targetLocation.X;
            Top = targetLocation.Y;

            _hoverTimer.Tick -= FirstHoverComplete;
            _hoverTimer.Tick += HoverComplete;
        }
        
        private void HoverComplete(object sender, EventArgs eventArgs)
        {
            _hoverTimer.Stop();
            _hoverTimer.Interval = TimeSpan.FromSeconds(.75);
            Left = targetLocation.X;
            Top = targetLocation.Y;
            if (!IsVisible)
            {
                Show();
            }


            WindowState = WindowState.Normal;
            Activate();
            Topmost = true; // important
            Topmost = false; // important
            Focus(); // important
        }


        public void HoverStop()
        {
            _hoverTimer.Stop();
            _hoverTimer.Interval = TimeSpan.FromSeconds(0.75);
        }

        public void ManualOpen(Point point, HWNDList hwnds)
        {
            _hoverTimer.Stop();
            _hoverTimer.Interval = TimeSpan.FromSeconds(.75);
            //Activate();
            Focus();
            Show();
            Left = point.X;
            Top = point.Y;
        }
    }
}