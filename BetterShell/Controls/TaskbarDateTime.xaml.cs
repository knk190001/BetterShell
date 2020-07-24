using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace BetterShell.Controls
{
    public partial class TaskbarDateTime : UserControl
    {
        private readonly string[] _months = new[]
            {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Oct", "Nov", "Dec"};

        private readonly string[] _weekdays = new[] {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"};

        public TaskbarDateTime()
        {
            InitializeComponent();
            var timer = new DispatcherTimer();
            timer.Tick += Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }    

        void Tick(object sender, EventArgs args)
        {
            var now = DateTime.Now;
            var day = now.Day;
            var month = _months[now.Month-1];
            var dayOfTheWeek = _weekdays[(int)now.DayOfWeek];
            Date.Text = $"{dayOfTheWeek}, {month} {day}";

            var time = now.ToShortTimeString();
            Time.Text = time;
        }
    }
}