namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    public interface IFolderOrFile
    {
        string AbsolutePath { get; }
        string Name { get; }
    }
}