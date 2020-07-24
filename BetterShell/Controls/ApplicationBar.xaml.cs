using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BetterShell.Controls
{
    public partial class ApplicationBar : UserControl
    {
        
        public ApplicationBar()
        {
            DataContext = this;
            InitializeComponent();
        }
    }

    public class TestData: ObservableCollection<string>
    {
        public TestData()
        {
            Add("Item 1");
            Add("Item 2");
            Add("Item 3");
        }
    }
}