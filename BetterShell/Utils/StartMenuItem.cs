using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using IWshRuntimeLibrary;

namespace BetterShell.Utils
{
    public class StartMenuItem : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            nameof(Name), typeof(string), typeof(StartMenuItem), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
            nameof(Path), typeof(string), typeof(StartMenuItem), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type), typeof(AppType), typeof(StartMenuItem), new PropertyMetadata(default(AppType)));


        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            nameof(Image), typeof(ImageSource), typeof(StartMenuItem), new PropertyMetadata(default(ImageSource)));

        public ImageSource Image
        {
            get => (ImageSource) GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public AppType Type
        {
            get => (AppType) GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public string Path
        {
            get => (string) GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }

        public string Name
        {
            get => (string) GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public StartMenuItem(string name, string path, AppType type)
        {
            Name = name;
            Path = path;
            Type = type;

            switch (type)
            {
                case AppType.Exe:
                    Image = GetExeMethod(path);
                    break;
                case AppType.AppX:
                    Image = GetAppXIcon(path);
                    break;
            }
        }

        private static ImageSource GetAppXIcon(string path)
        {
            return ApplicationUtils.GetIconForAppModelUserId(path);
        }

        private static BitmapSource GetExeMethod(string path)
        {
            try
            {
                var shell = new WshShell(); //Create a new WshShell Interface
                var link = (IWshShortcut) shell.CreateShortcut(path); //Link the interface to our shortcut
                var target = link.TargetPath;
                var fileIcon = Icon.ExtractAssociatedIcon(target);
                if (fileIcon != null)
                {
                    return Imaging.CreateBitmapSourceFromHIcon(fileIcon.Handle, Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                var fileIcon = Icon.ExtractAssociatedIcon(path);
                if (fileIcon != null)
                {
                    return Imaging.CreateBitmapSourceFromHIcon(fileIcon.Handle, Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
            }

            return null;
        }
    }

    public class StartMenuFolder : StartMenuItem
    {
        public List<StartMenuItem> Children { get; set; }

        public StartMenuFolder(string name, string path, AppType type) : base(name, path, type)
        {
            Children = new List<StartMenuItem>();
        }

        public void AddChild(StartMenuItem child)
        {
            if (Children.OfType<StartMenuFolder>().Any(o => o.Name == child.Name))
            {
                var startMenuFolder = Children.OfType<StartMenuFolder>().First(o => o.Name == child.Name);
                if (child is StartMenuFolder folder)
                {
                    folder.Children.ForEach(startMenuFolder.AddChild);
                }
            }
            else
            {
                Children.Add(child);
            }
        }
    }

    public class StartMenuLabel : StartMenuItem
    {
        public StartMenuLabel(char name) : base(name.ToString(), "", AppType.Label)
        {
        }
    }
}