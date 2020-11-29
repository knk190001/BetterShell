using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.UI.Xaml.Media;
using BetterShell.Utils.Win32Interop;
using BetterShell.Utils.WinRTInterop;
using Brushes = System.Windows.Media.Brushes;

namespace BetterShell.StartMenu
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class StartMenu:Window
    {
        private Window _w;
        private bool _canHide = true;
        private ApplicationActivationManager _applicationActivationManager;

        public StartMenu()
        {
            InitializeComponent();
            _applicationActivationManager = new ApplicationActivationManager();
        }

        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);
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
            Marshal.FreeHGlobal(accentPtr);
            
            
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (Application.Current.MainWindow != null && Application.Current.MainWindow.IsMouseOver)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void SetHrgn()
        {
            var hwnd = new WindowInteropHelper(this).Handle;

            var hwndSource = HwndSource.FromHwnd(hwnd);
            var sizeFactor = hwndSource.CompositionTarget.TransformToDevice.Transform(new Vector(1.0, 1.0));

            var dwmIsCompositionEnabled = NativeMethods.DwmIsCompositionEnabled();
            

            Background = Brushes.Transparent;
            hwndSource.CompositionTarget.BackgroundColor = Colors.Transparent;
            
            
            var blur = new NativeMethods.DWM_BLURBEHIND();
            blur.dwFlags = NativeMethods.DWM_BB.DWM_BB_ENABLE | NativeMethods.DWM_BB.DWM_BB_BLURREGION;
            blur.fEnable = true;
            
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, (int)(ActualWidth * sizeFactor.X), (int)(ActualHeight * sizeFactor.Y));
            var region = new Region(path);
            
            
            //NativeMethods.DwmEnableBlurBehindWindow(hwnd, ref blur);
            
            /*using (var path = new GraphicsPath())
            {
                path.AddEllipse(0, 0, (int)(ActualWidth * sizeFactor.X), (int)(ActualHeight * sizeFactor.Y));

                using (var region = new Region(path))
                using (var graphics = Graphics.FromHwnd(hwnd))
                {
                    var hRgn = region.GetHrgn(graphics);

                    User32.SetWindowRgn(hwnd, hRgn, true);
                    
                    var blur = new NativeMethods.DWM_BLURBEHIND
                    {
                        dwFlags = NativeMethods.DWM_BB.DWM_BB_ENABLE | NativeMethods.DWM_BB.DWM_BB_BLURREGION | NativeMethods.DWM_BB.DWM_BB_TRANSITIONONMAXIMIZED,
                        fEnable = true,
                        hRgnBlur = hRgn,
                        fTransitionOnMaximized = true
                    };

                    NativeMethods.DwmEnableBlurBehindWindow(hwnd, ref blur);
                    

                    region.ReleaseHrgn(hRgn);
                }
            }*/
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
            
            EnableBlur();
            SetHrgn();
        }
        
        
        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct DWM_BLURBEHIND
            {
                public DWM_BB dwFlags;
                public bool fEnable;
                public IntPtr hRgnBlur;
                public bool fTransitionOnMaximized;
            }

            [Flags]
            public enum DWM_BB
            {
                DWM_BB_ENABLE = 1,
                DWM_BB_BLURREGION = 2,
                DWM_BB_TRANSITIONONMAXIMIZED = 4
            }

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern bool DwmIsCompositionEnabled();

            [DllImport("dwmapi.dll", PreserveSig = false)]
            public static extern void DwmEnableBlurBehindWindow(IntPtr hwnd, ref DWM_BLURBEHIND blurBehind);
        }
    }
}