using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BetterShell.Utils.Win32Interop;

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
        
        public static string GetAppUserModelId(IShellItem applicationItem)
        {
            applicationItem.GetDisplayName(SIGDN.SIGDN_PARENTRELATIVEPARSING, out var pName);
            var name = Marshal.PtrToStringUni(pName);
            return name;
        }

        public static ImageSource GetIcon(IShellItem application)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var imageFactory = (IShellItemImageFactory) application;
            IntPtr hBitmap;
            if (imageFactory != null)
            {
                var hr = imageFactory.GetImage(new SIZE(50,50),SIIGBF.SIIGBF_ICONBACKGROUND, out hBitmap);
                ThrowExceptionForHR(hr);
            }
            else
            {
                return null;
            }

            var bmp = Imaging.CreateBitmapSourceFromHBitmap(hBitmap,IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

            return bmp;
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
        private static string TrueName(string path)
        {
            var info = new Shfileinfo();
            Shell32.SHGetFileInfo(path, 0, ref info, (uint) Marshal.SizeOf(info), 0x000000200);
            return info.szDisplayName;
        }

        private static string GetAppModelUserId(IShellItem applicationItem)
        {
            applicationItem.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING, out var pName);
            var name = Marshal.PtrToStringUni(pName);
            return name;
        }
        private static Tree<App> ToTree(IEnumerable<Dir> startMenu)
        { 
            var result = new Tree<App>();

            foreach (var dir in startMenu)
            {
                dir.ChildDirs
                    .Select(ToTree)
                    .ToList()
                    .ForEach(result.AddChildHierarchy);
                dir.ChildFiles
                    .Select(file => new App(){Path = file.FilePath,Type = AppType.Exe})
                    .ToList()
                    .ForEach(result.AddChild);
            }

            return result;
        }
        
        private static Branch<App> ToTree(Dir dir)
        {
            var result = new Branch<App>(new App(){Path = dir.FilePath,Type = AppType.None});
            
            dir.ChildDirs
                .Select(ToTree)
                .ToList()
                .ForEach(result.AddChildHierarchy);
            
            dir.ChildFiles
                .Select(file => new App(){Path = dir.FilePath,Type = AppType.None})
                .ToList()
                .ForEach(result.AddChild);
            
            return result;
        }
        
        public static Tree<App> GetStartMenu()
        {
            var fileStructure = new[]
                {
                    @"%AppData%\Microsoft\Windows\Start Menu\",
                    @"%AppData%\Microsoft\Windows\Start Menu\Programs",
                    @"%ProgramData%\Microsoft\Windows\Start Menu\",
                    @"%ProgramData%\Microsoft\Windows\Start Menu\Programs"
                }
                .Select(Environment.ExpandEnvironmentVariables)
                .Select(dir => new Dir(dir)).ToList();
            
            var executables = fileStructure
                .SelectMany(FileObject.GetAllFiles)
                .Select(o => o.FilePath)
                .Select(TrueName)
                .ToList();

            var shellItem = GetShellItem(ApplicationPath);
            var enumerator = GetEnumShellItems(shellItem);
            var uwpApps = EnumerateItems(enumerator)
                .Select(item => Tuple.Create(GetName(item), item))
                .Where(app => !executables.Contains(app.Item1))
                .Select(tuple => tuple.Item2)
                .Select(GetAppModelUserId)
                .Select(amuid => new App(){Path = amuid,Type = AppType.AppX})
                .ToList();

            var result = ToTree(fileStructure);
            
            uwpApps.ForEach(result.AddChild);
            
            return result;
            
        }
        
    }
}