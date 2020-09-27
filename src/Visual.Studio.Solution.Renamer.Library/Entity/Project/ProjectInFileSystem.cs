using System.IO;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Project
{
    public class ProjectInFileSystem : IProject
    {
        public ProjectInFileSystem(string absolutePath)
        {
            AbsolutePath = absolutePath;
            string fileName = Path.GetFileName(absolutePath);
            ProjectName = Path.GetFileNameWithoutExtension(fileName);
            Folder      = Path.GetDirectoryName(absolutePath) ?? "";
            Version     = null; // TODO: implement
        }

        public string AbsolutePath { get; }
        public string ProjectName { get; }
        public string Folder { get; }
        public string Version { get; }
    }
}