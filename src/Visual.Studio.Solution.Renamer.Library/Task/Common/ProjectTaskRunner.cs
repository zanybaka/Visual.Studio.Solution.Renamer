using System.IO;
using System.Linq;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public class ProjectTaskRunner : ITaskRunner
    {
        private readonly TargetProject _project;
        private readonly ProjectTaskCreator _taskCreator;

        public ProjectTaskRunner(TargetProject project, ProjectTaskCreator taskCreator)
        {
            _project     = project;
            _taskCreator = taskCreator;
        }

        public bool Run(bool preview)
        {
            Log.Information($"Task: {_taskCreator.Description}");
            UpdateProjectOptions options           = _project.Options;
            ProjectFinder        projectFinder     = new ProjectFinder();
            ProjectUpdater       projectUpdater    = new ProjectUpdater();
            bool                 atLeastOneUpdated = false;
            if (options.SolutionFullPath.IsNotEmpty())
            {
                var projects = projectFinder
                    .FindInTheSolution(
                        solutionDirectory: Path.GetDirectoryName(options.SolutionFullPath),
                        fileName: Path.GetFileName(options.SolutionFullPath))
                    .ToArray();

                projectUpdater.Run(
                    projects,
                    project =>
                    {
                        var task = _taskCreator.Create<ProjectInSolution>();
                        atLeastOneUpdated |= task.Run(project, (UpdateProjectOptions) options.WithPreview(preview));
                    });
            }
            else
            {
                var projects = projectFinder
                    .FindWithMask(
                        workingDirectory: options.WorkingDirectory,
                        options.CsProjFileMasks)
                    .ToArray();

                projectUpdater.Run(
                    projects,
                    project =>
                    {
                        var task = _taskCreator.Create<ProjectInFileSystem>();
                        atLeastOneUpdated |= task.Run(project, (UpdateProjectOptions) options.WithPreview(preview));
                    });
            }

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