using System.IO;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    public class FolderInFileSystem : IFolderOrFile
    {
        public FolderInFileSystem(string path)
        {
            AbsolutePath = path;
        }

        public string AbsolutePath { get; }
        public string Name => Path.GetDirectoryName(AbsolutePath);
    }
}