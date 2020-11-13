using System;
using System.Collections.Generic;
using System.Drawing;
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

        private static readonly IShellItem[] Applications = GetApplications();

        public static ImageSource GetIconForAppModelUserId(string appModelUserId)
        {
            return GetIcon(Applications.First(item => GetAppModelUserId(item) == appModelUserId));
        }
        
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
            // ReSharper disable once SuspiciousTypeConversion.Global
            var imageFactory = (IShellItemImageFactory) application;
            IntPtr hBitmap;
            if (imageFactory != null)
            {
                var hr = imageFactory.GetImage(new SIZE(50, 50), SIIGBF.SIIGBF_ICONBACKGROUND, out hBitmap);
                ThrowExceptionForHR(hr);
            }
            else
            {
                return null;
            }

            var bmp = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

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

        public static string GetAppModelUserId(IShellItem applicationItem)
        {
            applicationItem.GetDisplayName(SIGDN.SIGDN_DESKTOPABSOLUTEPARSING, out var pName);
            var name = Marshal.PtrToStringUni(pName);
            return name;
        }

        private static List<FileObject> GetAllFiles(FileObject fileObject)
        {
            var files = new List<FileObject>();

            if (!(fileObject is Dir dir)) return files;
            files.AddRange(dir.ChildDirs.SelectMany(GetAllFiles).ToList());
            files.AddRange(dir.ChildFiles);

            return files;
        }

        private static StartMenuFolder ToStartMenu(List<Dir> dirs)
        {
            var result = new StartMenuFolder(null, null, AppType.None);

            foreach (var dir in dirs)
            {
                dir.ChildDirs
                    .Select(ToStartMenu)
                    .ToList()
                    .ForEach(result.AddChild);

                dir.ChildFiles
                    .Select(file => new StartMenuItem(TrueName(file.FilePath), file.FilePath, AppType.Exe))
                    .Where(item => item.Name!="desktop.ini")
                    .ToList()
                    .ForEach(result.AddChild);
            }

            return result;
        }

        private static StartMenuItem ToStartMenu(Dir dir)
        {
            var result = new StartMenuFolder(TrueName(dir.FilePath), dir.FilePath, AppType.None);
            
            GetAllFiles(dir)
                .Where(o => o.Name !="Desktop.ini")
                .Where(o => o.Name !="desktop.ini")
                .Select(file => new StartMenuItem(TrueName(file.FilePath), file.FilePath, AppType.Exe))
                .ToList()
                .ForEach(result.AddChild);

            return result;
        }

        public static StartMenuFolder GetStartMenu()
        {
            var startMenu = new[]
                {
                    @"%AppData%\Microsoft\Windows\Start Menu\",
                    @"%AppData%\Microsoft\Windows\Start Menu\Programs",
                    @"%ProgramData%\Microsoft\Windows\Start Menu\",
                    @"%ProgramData%\Microsoft\Windows\Start Menu\Programs"
                }
                .Select(Environment.ExpandEnvironmentVariables)
                .Select(dir => new Dir(dir)).ToList();

            var files = startMenu
                .SelectMany(GetAllFiles)
                .Distinct()
                .AsParallel()
                .Select(o => o.FilePath)
                .Select(TrueName)
                .ToList();

            const string applicationPath = "shell:::{4234D49B-0245-4DF3-B780-3893943456E1}";

            var shellItem = GetShellItem(applicationPath);
            var enumShellItems = GetEnumShellItems(shellItem);
            var applications = EnumerateItems(enumShellItems)
                .Select(item => Tuple.Create(GetName(item), item))
                .Where(app => !files.Contains(app.Item1))
                .Select(tuple => Tuple.Create(tuple.Item1, GetAppModelUserId(tuple.Item2)))
                .Select(tuple => new StartMenuItem(tuple.Item1, tuple.Item2, AppType.AppX))
                .ToList();

            var tree = ToStartMenu(startMenu);

            tree.Children = tree.Children.FindAll(item => item.Name != "Programs");


            applications.ForEach(tree.AddChild);
            
            tree.Children
                .Select(o =>o.Name[0])
                .Select(char.ToLower)
                .Select(item => char.IsNumber(item) ? '#' : item)
                .Distinct()
                .Select(c => new StartMenuLabel(c))
                .Where(BranchesHaveChildren)
                .ToList()
                .ForEach(tree.AddChild);
            
            return tree;
        }

        private static bool BranchesHaveChildren(StartMenuItem item)
        {
            if (!(item is StartMenuFolder))
            {
                return true;
            }
            return ((StartMenuFolder) item).Children.Count != 0;
        }
        
        
        
    }
    
}