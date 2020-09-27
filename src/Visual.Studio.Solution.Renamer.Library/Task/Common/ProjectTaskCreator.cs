using System;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;

namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public abstract class ProjectTaskCreator : TaskCreatorBase
    {
        public AtPath<ProjectTaskCreator> At => new AtPath<ProjectTaskCreator>(this);

        public WithTargetProject With => new WithTargetProject(this);

        public InTargetSolution In => new InTargetSolution(this);

        protected abstract IProjectTask<ProjectInSolution> CreateTaskProjectInSolution();

        protected abstract IProjectTask<ProjectInFileSystem> CreateTaskProjectInFileSystem();

        internal IProjectTask<TProject> Create<TProject>() where TProject : IProject
        {
            if (typeof(TProject) == typeof(ProjectInFileSystem))
            {
                return (IProjectTask<TProject>) CreateTaskProjectInFileSystem();
            }

            if (typeof(TProject) == typeof(ProjectInSolution))
            {
                return (IProjectTask<TProject>) CreateTaskProjectInSolution();
            }

            throw new NotSupportedException($"Unknown type '{typeof(TProject)}'");
        }
    }
}