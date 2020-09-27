using System.IO;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Project
{
    public class ProjectInSolution : IProject
    {
        public string RawValue { get; set; }
        public string TypeGuid { get; set; }
        public string RelativePath { get; set; }
        public string Guid { get; set; }
        public string Content { get; set; }
        public string SolutionFilePath { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
        public string AbsolutePath => Path.Combine(Path.GetDirectoryName(SolutionFilePath) ?? "", RelativePath);
        public string Folder => Path.GetDirectoryName(AbsolutePath) ?? "";
    }
}