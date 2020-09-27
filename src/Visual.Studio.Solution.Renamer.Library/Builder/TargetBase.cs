using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Builder
{
    public abstract class TargetBase<TCreator, TOptions>
        where TCreator : TaskCreatorBase
        where TOptions : UpdateOptions
    {
        protected TargetBase(TOptions options, TCreator taskCreator)
        {
            Options     = options;
            TaskCreator = taskCreator;
        }

        internal TCreator TaskCreator { get; }

        internal TOptions Options { get; }

        protected abstract ITaskRunner CreateTaskRunner();

        public void Run(bool preview)
        {
            CreateTaskRunner().Run(preview);
        }

        public void Preview()
        {
            Run(true);
        }

        public void Apply()
        {
            Run(false);
        }
    }
}