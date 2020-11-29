using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using BetterShell.Utils;

namespace BetterShell.Controls
{
    public partial class ApplicationBarIcon : UserControl
    {
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(ApplicationBarIcon));
        // public static DependencyProperty MultipleProperty = DependencyProperty.Register("Multiple",typeof(bool),typeof(ApplicationBarIcon));
        // public static DependencyProperty IsPrimaryProperty = DependencyProperty.Register("IsPrimary",typeof(bool),typeof(ApplicationBarIcon));

        public static readonly DependencyProperty ToolTipTextProperty = DependencyProperty.Register(
            nameof(ToolTipText), typeof(string), typeof(ApplicationBarIcon), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HWNDProperty = DependencyProperty.Register(
            nameof(HWND), typeof(HWNDList), typeof(ApplicationBarIcon), new PropertyMetadata(default(HWNDList)));

        public ApplicationBarIcon()
        {
            InitializeComponent();
        }

        public string ToolTipText
        {
            get => (string) GetValue(ToolTipTextProperty);
            set => SetValue(ToolTipTextProperty, value);
        }

        public HWNDList HWND
        {
            get => (HWNDList) GetValue(HWNDProperty);
            set => SetValue(HWNDProperty, value);
        }

        public ImageSource Icon
        {
            get => (ImageSource) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            var loc = PointToScreen(new Point(ActualWidth / 2, 0));
            var source = PresentationSource.FromVisual(this);
            if (source == null) return;
            if (source.CompositionTarget == null) return;
            var targetVec = source.CompositionTarget.TransformFromDevice.Transform(loc);

            var taskbarHoverWindow = TaskbarHoverWindow.TaskbarHoverWindow.Instance;
            var point = new Point(targetVec.X - taskbarHoverWindow.Width / 2, targetVec.Y - taskbarHoverWindow.Height);
            TaskbarHoverWindow.TaskbarHoverWindow.Instance.StartHover(point, HWND);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            TaskbarHoverWindow.TaskbarHoverWindow.Instance.HoverStop();
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            if (HWND.Count == 1)
            {
                RunningApplicationUtils.SetForegroundWindow(HWND[0]);
            }
            else
            {
                var loc = PointToScreen(new Point(ActualWidth / 2, 0));
                var source = PresentationSource.FromVisual(this);
                if (source == null) return;
                if (source.CompositionTarget == null) return;
                var targetVec = source.CompositionTarget.TransformFromDevice.Transform(loc);

                var taskbarHoverWindow = TaskbarHoverWindow.TaskbarHoverWindow.Instance;
                var point = new Point(targetVec.X - taskbarHoverWindow.Width / 2,
                    targetVec.Y - taskbarHoverWindow.Height);
                taskbarHoverWindow.ManualOpen(point, HWND);
            }
        }
    }
}