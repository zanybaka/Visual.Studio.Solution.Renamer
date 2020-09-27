using System.IO;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    public class FileInFileSystem : IFolderOrFile
    {
        public FileInFileSystem(string path)
        {
            AbsolutePath = path;
        }

        public string AbsolutePath { get; }
        public string Name => Path.GetFileName(AbsolutePath);
    }
}