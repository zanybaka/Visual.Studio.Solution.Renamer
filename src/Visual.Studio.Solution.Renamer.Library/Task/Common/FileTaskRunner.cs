using System.Linq;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public class FileTaskRunner<TCreator> : ITaskRunner
        where TCreator : TaskCreatorBase
    {
        private readonly TargetFile<TCreator> _file;
        private readonly FolderOrFileTaskCreator<TCreator> _taskCreator;

        public FileTaskRunner(TargetFile<TCreator> file, FolderOrFileTaskCreator<TCreator> taskCreator)
        {
            _file        = file;
            _taskCreator = taskCreator;
        }

        public bool Run(bool preview)
        {
            Log.Information($"Task: {_taskCreator.Description}");
            UpdateFileOptions options = _file.Options;
            FileFinder        finder  = new FileFinder();
            FileUpdater       updater = new FileUpdater();
            var files = finder
                .FindWithMask(
                    options.WorkingDirectory,
                    options.Masks,
                    options.Recursively)
                .ToArray();

            bool atLeastOneUpdated = false;
            updater.Run(
                files,
                folder =>
                {
                    IFolderOrFileTask task = _taskCreator.Create();
                    atLeastOneUpdated |= task.Run(folder, options.WithPreview(preview));
                });
            if (!atLeastOneUpdated) Log.Information("\tTask completed. No changes.");
            return atLeastOneUpdated;
        }

        public bool Preview()
        {
            return Run(true);
        }

        public bool Apply()
        {
            return Run(false);
        }
    }
}