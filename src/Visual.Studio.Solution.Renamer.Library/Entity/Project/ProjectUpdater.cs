using System;
using Serilog;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Project
{
    internal class ProjectUpdater
    {
        public void Run<TProject>(TProject[] projects, Action<TProject> updateAction)
            where TProject : IProject
        {
            Log.Verbose($"Found {projects.Length} projects");
            foreach (var project in projects)
            {
                Log.Verbose($"Updating project '{project.ProjectName}'");
                try
                {
                    updateAction(project);
                }
                catch (Exception e)
                {
                    Log.Error(e, "FAILED");
                }
            }

            Log.Verbose("Updated.");
        }
    }
}