using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Replaces project default RootNamespace with the project name
    /// </summary>
    internal class SyncProjectDefaultRootNamespaceTask : IProjectTask<IProject>
    {
        public bool Run(IProject project, UpdateProjectOptions options)
        {
            return
                new SyncXmlSingleNodeHelper()
                    .AddOrUpdate(
                        project.AbsolutePath,
                        "RootNamespace",
                        project.ProjectName.Replace("-", "_"),
                        options.Preview);
        }
    }
}