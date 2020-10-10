using System;
using System.IO;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Extension;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Renames project folder with the project name
    /// </summary>
    internal class SyncFolderNameTask : IProjectTask<IProject>
    {
        public bool Run(IProject project, UpdateProjectOptions options)
        {
            if (AreNamesSynced(project.ProjectName, project.Folder))
            {
                Log.Verbose($"Skipped because of its name equality with the folder {project.Folder}");
                return false;
            }

            if (project.Folder.NormalizePath().Equals(options.WorkingDirectory, StringComparison.InvariantCultureIgnoreCase))
            {
                Log.Verbose("Skipped because the project is placed in the same folder as its solution.");
                return false;
            }

            string projectNewFolder = Path.Combine(Path.GetDirectoryName(project.Folder) ?? "", project.ProjectName);
            return UpdateFolderName(project.Folder, projectNewFolder, options);
        }

        private static bool UpdateFolderName(string currentFolder, string newFolder, UpdateProjectOptions options)
        {
            if (currentFolder.Equals(newFolder, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            Log.Verbose($"Renaming folder '{currentFolder}' to '{newFolder}'");
            if (!options.Preview)
            {
                Directory.Move(currentFolder, newFolder);
            }

            Log.Verbose("Renamed.");
            return true;
        }

        private static bool AreNamesSynced(string projectName, string oldFolder)
        {
            return oldFolder.EndsWith(projectName, ignoreCase: true, culture: null);
        }
    }
}