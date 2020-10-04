using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Visual.Studio.Solution.Renamer.Library.Entity.Project;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.Library.Entity
{
    /// <remarks>
    ///     Alternative parsers
    ///     1. you can user Microsoft.Build.Construction.SolutionFile for parsing
    ///     2. https://github.com/ost-onion/SolutionParser
    /// </remarks>
    public class SolutionFile
    {
        private const string ProjectPattern =
            @"Project\(""(?<typeGuid>.*?)""\)\s+=\s+""(?<name>.*?)"",\s+""(?<path>.*?)"",\s+""(?<guid>.*?)""(?<content>.*?)\bEndProject";

        private const string ProjectFormat = @"Project(""{0}"") = ""{1}"", ""{2}"", ""{3}""{4}EndProject";
        public readonly string FilePath;
        public string FileContent;
        public ProjectInSolution[] ProjectsInOrder;

        private SolutionFile(string fullPath, string fileContent)
        {
            fullPath.AssertFileExists();
            FilePath    = fullPath;
            FileContent = fileContent;
        }

        public static bool Validate(string fullPath)
        {
            SolutionFile file = Parse(fullPath);

            ProjectInSolution project = file.ProjectsInOrder.FirstOrDefault(x => x.RelativePath.Contains(".."));
            if (project != null)
            {
                return false;
            }

            return true;
        }

        public static SolutionFile Parse(string fullPath)
        {
            Regex regex = new Regex(
                ProjectPattern,
                RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            string fileContent = File.ReadAllText(fullPath);
            var projects = regex.Matches(fileContent)
                .OfType<Match>()
                .Select(x => new ProjectInSolution
                {
                    RelativePath     = x.Groups["path"].Value,
                    Content          = x.Groups["content"].Value,
                    Guid             = x.Groups["guid"].Value,
                    ProjectName      = x.Groups["name"].Value,
                    TypeGuid         = x.Groups["typeGuid"].Value,
                    RawValue         = x.Groups[0].Value,
                    SolutionFilePath = fullPath
                })
                .ToArray();

            return new SolutionFile(fullPath, fileContent)
            {
                ProjectsInOrder = projects
            };
        }

        public void UpdateProject(ProjectInSolution project)
        {
            FileContent = FileContent.Replace(
                project.RawValue,
                string.Format(ProjectFormat, project.TypeGuid, project.ProjectName,
                              project.RelativePath,
                              project.Guid, project.Content),
                StringComparison.InvariantCultureIgnoreCase);
        }

        public void SaveToDisk()
        {
            File.WriteAllText(FilePath, FileContent);
        }
    }
}