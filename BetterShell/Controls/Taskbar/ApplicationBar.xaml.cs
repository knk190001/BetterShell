using System.Collections.ObjectModel;
using System.Linq;
using BetterShell.Utils;
using UserControl = System.Windows.Controls.UserControl;


namespace BetterShell.Controls
{
    public partial class ApplicationBar : UserControl
    {
        public Applications Applications { get; set; }

        public ApplicationBar()
        {
            Applications = new Applications();
            InitializeComponent();
        }
    }


    

    public class Applications : ObservableCollection<TaskbarApplication>
    {
        public Applications()
        {
            var applications = RunningApplicationUtils.OpenWindows();
            var iconsTask = RunningApplicationUtils.GetIcons(applications);
            for (var i = 0; i < applications.Count; i++)
            {
                var id = RunningApplicationUtils.GetIdentifier(applications[i]);

                if (ContainsIdentifier(id, out var app))
                {
                    app.Count++;
                    continue;
                }

                Add(new TaskbarApplication()
                {
                    Count = 1,
                    Icon = iconsTask[i],
                    Identifier = RunningApplicationUtils.GetIdentifier(applications[i]),
                    HWND = applications[i].hwnd
                });
            }
        }

        private bool ContainsIdentifier(string id, out TaskbarApplication wrapperIfAny)
        {
            wrapperIfAny = this.FirstOrDefault(wrapper => wrapper.Identifier == id);
            return wrapperIfAny != null;
        }
        
        
        

        

        


        

        // ReSharper disable IdentifierTypo
    }
}