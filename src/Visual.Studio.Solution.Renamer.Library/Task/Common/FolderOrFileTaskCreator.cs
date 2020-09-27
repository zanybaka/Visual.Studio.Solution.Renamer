using Visual.Studio.Solution.Renamer.Library.Builder;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public abstract class FolderOrFileTaskCreator<TCreator> : TaskCreatorBase
        where TCreator : TaskCreatorBase
    {
        public abstract AtPath<TCreator> At { get; }

        public WithTargetFolderOrFile<TCreator> In => new WithTargetFolderOrFile<TCreator>(this);

        internal abstract IFolderOrFileTask Create();
    }
}