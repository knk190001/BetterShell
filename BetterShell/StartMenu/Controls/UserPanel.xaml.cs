﻿using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using BetterShell.Utils.WinRTInterop;

namespace BetterShell.StartMenu.Controls
{
    public partial class UserPanel : UserControl
    {
        private IApplicationActivationManager _applicationActivationManager;

        public UserPanel()
        {
            InitializeComponent();
            _applicationActivationManager = new ApplicationActivationManager();
        }

        private void Office_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationActivationManager.ActivateApplication(
                "Microsoft.MicrosoftOfficeHub_8wekyb3d8bbwe!Microsoft.MicrosoftOfficeHub", null, ActivateOptions.None,
                out var pid);
        }

        private void OneDrive_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", @"shell:::{018D5C66-4533-4307-9B53-224DE2ED1FE6}");
        }
    }
}