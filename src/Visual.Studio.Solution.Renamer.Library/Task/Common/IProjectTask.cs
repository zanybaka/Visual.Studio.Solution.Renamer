using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public interface IProjectTask<in TProject> : ITask
        where TProject : IProject
    {
        bool Run(TProject project, UpdateProjectOptions options);
    }
}