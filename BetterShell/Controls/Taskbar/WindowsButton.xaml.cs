using System.Windows;
using System.Windows.Controls;

namespace BetterShell.Controls
{
    public partial class WindowsButton : UserControl
    {
        private readonly StartMenu.StartMenu _window;
        
        public WindowsButton()
        {
            InitializeComponent();
            _window = new StartMenu.StartMenu {Left = 0, Top = 270};
        }

        private void StartMenu_OnClick(object sender, RoutedEventArgs e)
        {
            ToggleWindow();
        }

        private void ToggleWindow()
        {
            if (_window.IsVisible)
            {
                _window.Hide();
            }
            else
            {
                _window.Show();
            }
            
        }
    }
}