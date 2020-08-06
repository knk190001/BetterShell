﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace BetterShell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ImageSource WallPaper
        {
            get;
            set;
        }


        public MainWindow()
        {
            InitializeComponent();

            
            var bg = Registry.GetValue(@"HKEY_CURRENT_USER\Control Panel\Desktop","WallPaper",null);
            var uri = new Uri(bg.ToString(),UriKind.Absolute);

            WallPaper = new BitmapImage(uri);
        }

        
    }
}
