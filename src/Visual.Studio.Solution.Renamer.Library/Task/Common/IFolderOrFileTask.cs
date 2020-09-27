using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public interface IFolderOrFileTask : ITask
    {
        bool Run(IFolderOrFile entity, UpdateOptions options);
    }
}