using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    internal class SyncFolderNameAndProjectNameTask : IProjectTask<ProjectInSolution>
    {
        public bool Run(ProjectInSolution project, UpdateProjectOptions options)
        {
            var res = new SyncFolderNameTask().Run(project, options)
                      | new SyncProjectNameTask().Run(project, options);
            return res;
        }
    }
}