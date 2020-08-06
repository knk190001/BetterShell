using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using BetterShell.Utils;
using BetterShell.Utils.Win32Interop;

namespace BetterShell.StartMenu.Controls
{
    public partial class AppList : UserControl
    {
        private ObservableCollection<ApplicationGroup> Applications { get; set; }

        public AppList()
        {
            InitializeComponent();

            Applications = new ObservableCollection<ApplicationGroup>();
            // ReSharper disable once StringLiteralTypo
            foreach (var s in "#ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray())
            {
                Applications.Add(new ApplicationGroup() {Letter = s.ToString(), Applications = new ObservableCollection<Application>()});
            }

            var applications = ApplicationUtils.GetApplications();
            foreach (var shellItem in applications)
            {
                var name = ApplicationUtils.GetName(shellItem);
                var icon = ApplicationUtils.GetIcon(shellItem);
                foreach (var applicationGroup in Applications)
                {
                    if (applicationGroup.Letter == name[0].ToString() ||
                        applicationGroup.Letter == "#" && char.IsDigit(name[0]))
                    {
                        applicationGroup.Applications.Add(new Application() {Name = name, Icon = icon});
                    }
                }
            }

            DataContext = Applications;
        }
    }

    public class ApplicationGroup
    {
        public string Letter { get; set; }
        public ObservableCollection<Application> Applications { get; set; }
    }

    public class Application
    {
        public string Name { get; set; }
        public ImageSource Icon { get; set; }
    }
}