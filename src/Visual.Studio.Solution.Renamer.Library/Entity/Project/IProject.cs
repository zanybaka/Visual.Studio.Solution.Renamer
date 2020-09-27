namespace Visual.Studio.Solution.Renamer.Library.Entity.Project
{
    public interface IProject
    {
        string AbsolutePath { get; }
        string ProjectName { get; }
        string Folder { get; }
        string Version { get; }
    }
}