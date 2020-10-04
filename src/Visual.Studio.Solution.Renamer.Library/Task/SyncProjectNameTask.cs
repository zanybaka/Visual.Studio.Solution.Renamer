using System;
using System.IO;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Extension;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Renames project file and updates its solution file
    /// </summary>
    internal class SyncProjectNameTask : IProjectTask<ProjectInSolution>
    {
        public bool Run(ProjectInSolution project, UpdateProjectOptions options)
        {
            bool updated;
            if (!string.IsNullOrEmpty(options.ReplaceNameFrom) && !string.IsNullOrEmpty(options.ReplaceNameTo))
            {
                project.ProjectName = project.ProjectName.Replace(options.ReplaceNameFrom, options.ReplaceNameTo, StringComparison.InvariantCultureIgnoreCase);
                var oldRelativeDir = project.Folder.GetRelativePath(Path.GetDirectoryName(options.SolutionFullPath));
                updated = UpdateProject(oldRelativeDir, project.Folder, project, options.Preview, keepCsProjFileName: false);
            }
            else
            {
                string projectNewFolder = Path.Combine(Path.GetDirectoryName(project.Folder) ?? "", project.ProjectName);
                var    newRelativeDir   = projectNewFolder.GetRelativePath(Path.GetDirectoryName(options.SolutionFullPath));
                updated = UpdateProject(newRelativeDir, projectNewFolder, project, options.Preview, keepCsProjFileName: false);
            }

            if (!updated)
            {
                return false;
            }

            SolutionFile slnFile = SolutionFile.Parse(options.SolutionFullPath);
            UpdateSolutionFile(project, slnFile, options.Preview);
            return true;
        }

        private static bool UpdateProject(string newRelativeDir,
                                          string projectNewFolder,
                                          ProjectInSolution project,
                                          bool preview,
                                          bool keepCsProjFileName = false)
        {
            Log.Verbose($"Replacing project RelativePath '{project.RelativePath}' to");
            string currentCsProjFileName = Path.GetFileName(project.RelativePath);
            if (keepCsProjFileName)
            {
                string newValue = Path.Combine(newRelativeDir, currentCsProjFileName);
                Log.Verbose($"'{newValue}'");
                if (project.RelativePath == newValue)
                {
                    Log.Verbose("No changes.");
                    return false;
                }

                project.RelativePath = newValue;
            }
            else
            {
                string newCsProjFileName      = $"{project.ProjectName}{ProjectConstants.ProjectExtension}";
                string projectNewRelativePath = Path.Combine(newRelativeDir, newCsProjFileName);
                Log.Verbose($"'{projectNewRelativePath}'");
                if (currentCsProjFileName == newCsProjFileName && project.RelativePath == projectNewRelativePath)
                {
                    Log.Verbose("No changes.");
                    return false;
                }

                if (!preview)
                {
                    File.Move(Path.Combine(projectNewFolder, currentCsProjFileName), Path.Combine(projectNewFolder, newCsProjFileName));
                }

                project.RelativePath = projectNewRelativePath;
            }

            Log.Verbose("Replaced.");
            return true;
        }

        private static void UpdateSolutionFile(ProjectInSolution project,
                                               SolutionFile slnFile,
                                               bool preview)
        {
            Log.Verbose($"Updating the solution file '{slnFile.FilePath}'");
            slnFile.UpdateProject(project);
            if (!preview)
            {
                slnFile.SaveToDisk();
            }

            Log.Verbose("Updated.");
        }
    }
}