using System.Collections.Generic;
using System.IO;
using System.Linq;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Project
{
    internal class ProjectFinder
    {
        public IEnumerable<ProjectInFileSystem> FindWithMask(string workingDirectory, string[] masks)
        {
            if (workingDirectory == null || masks == null || masks.Length == 0)
            {
                return Enumerable.Empty<ProjectInFileSystem>();
            }

            var projects = masks
                .SelectMany(m => Directory.EnumerateFiles(workingDirectory, m, SearchOption.AllDirectories))
                .Select(f => new ProjectInFileSystem(f));

            return projects;
        }

        public IEnumerable<ProjectInSolution> FindInTheSolution(string solutionDirectory, string fileName)
        {
            if (solutionDirectory == null || fileName == null)
            {
                return Enumerable.Empty<ProjectInSolution>();
            }

            string       slnPath  = Path.Combine(solutionDirectory, fileName);
            SolutionFile slnFile  = SolutionFile.Parse(slnPath);
            var          projects = slnFile.ProjectsInOrder.Where(x => x.IsCsProj());

            return projects;
        }
    }
}