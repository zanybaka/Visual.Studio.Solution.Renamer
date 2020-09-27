using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Replaces project default AssemblyName with the project name
    /// </summary>
    internal class SyncProjectDefaultAssemblyNameTask : IProjectTask<IProject>
    {
        public bool Run(IProject project, UpdateProjectOptions options)
        {
            return
                new SyncXmlSingleNodeHelper()
                    .AddOrUpdate(
                        project.AbsolutePath,
                        "AssemblyName",
                        project.ProjectName,
                        options.Preview);
        }
    }
}