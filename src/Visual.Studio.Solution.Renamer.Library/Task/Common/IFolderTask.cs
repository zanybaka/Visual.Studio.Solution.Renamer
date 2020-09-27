using Visual.Studio.Solution.Renamer.Library.Entity.Folder;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public interface IFolderTask : ITask
    {
        bool Run(FolderInFileSystem entity, UpdateFolderOptions options);
    }
}