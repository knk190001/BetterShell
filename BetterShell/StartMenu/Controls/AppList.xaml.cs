using System;
using System.Windows.Controls;
using BetterShell.Utils;

namespace BetterShell.StartMenu.Controls
{
    public partial class AppList : UserControl
    {
        
        public AppList()
        {
            InitializeComponent();
            var startMenuItems = ApplicationUtils.GetStartMenu().Children;
            startMenuItems.Sort((item1, item2) => string.Compare(item1.Name,item2.Name,StringComparison.CurrentCultureIgnoreCase) );
            StartMenu.ItemsSource = startMenuItems;
        }
    }
}