using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BetterShell.Utils
{
    public enum FileAttr
    {
        File,
        Directory
    }

    public abstract class FileObject
    {
        public abstract string FilePath { get; }
        public abstract string Name { get; }

        public abstract FileAttr Type { get; }

        public abstract List<FileObject> Children { get; }
        
        public static List<FileObject> GetAllFiles(FileObject fileObject)
        {
            var files = new List<FileObject>();

            if (!(fileObject is Dir dir)) return files;
            files.AddRange(dir.ChildDirs.SelectMany(GetAllFiles).ToList());
            files.AddRange(dir.ChildFiles);

            return files;
        }
    }

    public sealed class FileWrapper : FileObject
    {
        public override string FilePath { get; }
        public override string Name { get; }
        public override FileAttr Type { get; }
        public override List<FileObject> Children { get; }

        public FileWrapper(string filePath)
        {
            FilePath = filePath;
            Name = Path.GetFileName(filePath);
            Type = FileAttr.File;
            Children = null;
        }
    }

    public sealed class Dir : FileObject
    {
        public override string FilePath { get; }
        public override string Name { get; }
        public override FileAttr Type { get; }
        public override List<FileObject> Children { get; }

        public List<Dir> ChildDirs => Children != null ? Children.OfType<Dir>().ToList() : new List<Dir>();

        public List<FileWrapper> ChildFiles =>
            Children != null ? Children.OfType<FileWrapper>().ToList() : new List<FileWrapper>();

        public Dir(string path)
        {
            FilePath = path;
            Name = Path.GetDirectoryName(path);
            Type = FileAttr.Directory;

            var files = Directory.GetFiles(path);
            var subDirectories = Directory.GetDirectories(path);

            Children = files.Select(file => new FileWrapper(file))
                .Cast<FileObject>()
                .Union(
                    subDirectories.Select(subDirectory => new Dir(subDirectory))
                ).ToList();
        }
    }
}