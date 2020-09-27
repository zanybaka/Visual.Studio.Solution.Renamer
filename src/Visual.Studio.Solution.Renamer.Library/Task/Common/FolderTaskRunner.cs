﻿using System.Linq;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public class FolderTaskRunner<TCreator> : ITaskRunner
        where TCreator : TaskCreatorBase
    {
        private readonly TargetFolder<TCreator> _folder;
        private readonly FolderOrFileTaskCreator<TCreator> _taskCreator;

        public FolderTaskRunner(TargetFolder<TCreator> folder, FolderOrFileTaskCreator<TCreator> taskCreator)
        {
            _folder      = folder;
            _taskCreator = taskCreator;
        }

        public bool Run(bool preview)
        {
            Log.Information($"Task: {_taskCreator.Description}");
            UpdateFolderOptions options = _folder.Options;
            FolderFinder        finder  = new FolderFinder();
            FolderUpdater       updater = new FolderUpdater();
            var folders = finder
                .FindWithMask(
                    options.WorkingDirectory,
                    options.Masks,
                    options.Recursively)
                .ToArray();

            bool atLeastOneUpdated = false;
            updater.Run(
                folders,
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