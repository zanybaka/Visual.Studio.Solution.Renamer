using System;
using CommandLine;
using Visual.Studio.Solution.Renamer.Library;

namespace Visual.Studio.Solution.Renamer.CommandLine
{
    public class CommandLineOptions : IRenameOptions
    {
        [Option('a', "apply", Required = false,
                HelpText               = "Disable preview mode and apply all the changes in the file system (Preview mode is enabled by default)")]
        public bool Apply { get; set; }

        [Option('m', "mask", Required = false, HelpText = "Set the list of file masks to be processed. Default is *.csproj *.cs *.xaml *.xml *.json")]
        public string Mask { get; set; }

        public bool Preview
        {
            get => !Apply;
            set => Apply = !value;
        }

        [Option('f', "from", Required = false, HelpText = "Set 'From' field for renaming.")]
        public string ReplaceFrom { get; set; }

        [Option('t', "to", Required = false, HelpText = "Set 'To' field for renaming.")]
        public string ReplaceTo { get; set; }

        [Option('c', "cleanup", Required = false, HelpText = "Remove 'bin' and 'obj' directories.")]
        public bool Cleanup { get; set; }

        [Option('p', "projects", Required = false, HelpText = "Ignore .sln file and use .csproj files directly.")]
        public bool UseCsProj { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('s', "solution", Required = false, HelpText = "Set .sln file.")]
        public string SolutionFile { get; set; }

        public string[] Masks
        {
            get => Mask.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            set => Mask = string.Join(' ', value);
        }

        [Option('w', "workingdirectory", Required = false, HelpText = "Set working directory.")]
        public string WorkingDirectory { get; set; }
    }
}