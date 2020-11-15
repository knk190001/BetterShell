using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using Windows.UI.Xaml.Media;
using BetterShell.Utils.Win32Interop;
using BetterShell.Utils.WinRTInterop;

namespace BetterShell.StartMenu
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StartMenu
    {
        private Window _w;
        private bool _canHide = true;
        private ApplicationActivationManager _applicationActivationManager;

        public StartMenu()
        {
            InitializeComponent();
            _applicationActivationManager = new ApplicationActivationManager();
            EnableBlur();
        }

        private void EnableBlur()
        {
            /*var windowHelper = new WindowInteropHelper(this);
            var accent = new AccentPolicy();
            var accentSize = Marshal.SizeOf(accent);
            
            
            accent.AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND;
            
            
            var accentPtr = Marshal.AllocHGlobal(accentSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentSize,
                Data = accentPtr
            };

            var res = User32.SetWindowCompositionAttribute(windowHelper.Handle, ref data);
            if (res == 0)
            {
                throw new Exception("Error setting blurred background: "+Marshal.GetLastWin32Error());
            }
            Marshal.FreeHGlobal(accentPtr);*/
            
            
        }

        private void Office_OnClick(object sender, RoutedEventArgs e)
        {
            _applicationActivationManager.ActivateApplication(
                "Microsoft.MicrosoftOfficeHub_8wekyb3d8bbwe!Microsoft.MicrosoftOfficeHub", null, ActivateOptions.None,
                out var pid);
        }

        private void OneDriver_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", @"shell:::{018D5C66-4533-4307-9B53-224DE2ED1FE6}");
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            if (Application.Current.MainWindow != null) Left = Application.Current.MainWindow.Left;
        }
    }
}