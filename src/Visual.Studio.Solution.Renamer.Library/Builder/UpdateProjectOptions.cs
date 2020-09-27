using System.IO;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.Library.Builder
{
    public class UpdateProjectOptions : UpdateOptions
    {
        private UpdateProjectOptions()
        {
        }

        public string SolutionFullPath { get; private set; }
        public string ReplaceNameFrom { get; private set; }
        public string ReplaceNameTo { get; private set; }
        public string[] CsProjFileMasks { get; private set; }

        public UpdateProjectOptions WithReplace(string from, string to)
        {
            ReplaceNameFrom = from;
            ReplaceNameTo   = to;
            return this;
        }

        public UpdateProjectOptions WithSolution(string directory, string slnPath)
        {
            if (!Path.IsPathRooted(slnPath))
            {
                slnPath = Path.Combine(directory, slnPath);
            }

            slnPath.AssertFileExists();
            WorkingDirectory = directory;
            SolutionFullPath = slnPath;
            return this;
        }

        public UpdateProjectOptions WithCsProjMasks(params string[] value)
        {
            CsProjFileMasks = value;
            return this;
        }

        public static UpdateProjectOptions Empty()
        {
            return new UpdateProjectOptions();
        }

        public static UpdateProjectOptions ProjectsFromTheSolution(string directory, string slnPath)
        {
            directory.AssertDirectoryExists();
            if (!Path.IsPathRooted(slnPath))
            {
                slnPath = Path.Combine(directory, slnPath);
            }

            slnPath.AssertFileExists();
            return new UpdateProjectOptions { WorkingDirectory = directory, SolutionFullPath = slnPath };
        }

        public static UpdateProjectOptions AllProjects(string directory)
        {
            return ProjectsWithMask(directory, new[] { $"*{ProjectConstants.ProjectExtension}" });
        }

        public static UpdateProjectOptions ProjectsWithMask(string directory, string[] fileMasks)
        {
            directory.AssertDirectoryExists();
            fileMasks.AssertAtLeastOneElement();
            foreach (string fileMask in fileMasks)
            {
                fileMask.AssertEndsWith(ProjectConstants.ProjectExtension);
            }

            return new UpdateProjectOptions { WorkingDirectory = directory, CsProjFileMasks = fileMasks };
        }

        public UpdateProjectOptions Clone()
        {
            return (UpdateProjectOptions) MemberwiseClone();
        }
    }
}