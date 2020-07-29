using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.UI.Xaml.Controls;
using BetterShell.Utils.Win32Interop;
using XamlCSS.CssParsing;
using Image = System.Drawing.Image;

namespace BetterShell.Utils
{
    public static class ApplicationUtils
    {
        private const string ApplicationPath = "shell:::{4234D49B-0245-4DF3-B780-3893943456E1}";

        public static IShellItem[] GetApplications()
        {
            var applicationsFolder = GetShellItem(ApplicationPath);
            var applicationEnumerator = GetEnumShellItems(applicationsFolder);
            var applications = EnumerateItems(applicationEnumerator);

            return applications.ToArray();
        }

        public static string GetName(IShellItem applicationItem)
        {
            applicationItem.GetDisplayName(SIGDN.SIGDN_NORMALDISPLAY, out var pName);
            var name = Marshal.PtrToStringUni(pName);
            return name;
        }

        public static ImageSource GetIcon(IShellItem application)
        {
            var imageFactory = application as IShellItemImageFactory;
            var hr = imageFactory.GetImage(new SIZE(96,96),SIIGBF.SIIGBF_BIGGERSIZEOK, out var hBitmap);
            ThrowExceptionForHR(hr);
            var bmp = Image.FromHbitmap(hBitmap);
            using (Stream str = new MemoryStream())
            {
                bmp.Save(str, System.Drawing.Imaging.ImageFormat.Bmp);
                str.Seek(0, SeekOrigin.Begin);
                BitmapDecoder bdc = new BmpBitmapDecoder(str, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);
                return bdc.Frames[0];
            }
        }
        
        private static IShellItem GetShellItem(string path)
        {
            uint rgfInOut = 0;
            var hr = Shell32.SHILCreateFromPath(path, out var pShellItem, ref rgfInOut);

            ThrowExceptionForHR(hr);

            Shell32.SHCreateItemFromIDList(pShellItem, typeof(IShellItem).GUID, out var item);

            return item;
        }

        // ReSharper disable once InconsistentNaming
        private static void ThrowExceptionForHR(HRESULT hr)
        {
            if (hr != HRESULT.S_OK)
            {
                Marshal.ThrowExceptionForHR((int) hr);
            }
        }

        private static IEnumShellItems GetEnumShellItems(IShellItem shellItem)
        {
            var guidEnumItems = new Guid("94f60519-2850-4924-aa5a-d15e84868039");
            var hr = shellItem.BindToHandler(IntPtr.Zero, guidEnumItems, typeof(IEnumShellItems).GUID,
                out var pEnumItems);
            ThrowExceptionForHR(hr);
            return Marshal.GetObjectForIUnknown(pEnumItems) as IEnumShellItems;
        }
        
        private static IShellItem[] EnumerateItems(IEnumShellItems enumShellItems)
        {
            var items = new List<IShellItem>();

            while (enumShellItems.Next(1, out var item, out var nFetched) == HRESULT.S_OK && nFetched == 1)
            {
                items.Add(item);
            }

            return items.ToArray();
        }
    }
}