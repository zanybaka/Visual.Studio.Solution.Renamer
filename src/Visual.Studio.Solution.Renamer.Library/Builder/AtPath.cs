using Visual.Studio.Solution.Renamer.Library.Extension;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Builder
{
    public class AtPath<TCreator>
        where TCreator : TaskCreatorBase
    {
        private readonly TCreator _taskCreator;

        public AtPath(TCreator taskCreator)
        {
            _taskCreator = taskCreator;
        }

        public TCreator Path(string path)
        {
            path.AssertDirectoryExists();
            _taskCreator.Path = path;
            return _taskCreator;
        }
    }
}