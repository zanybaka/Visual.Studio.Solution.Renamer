using System;
using CommandLine;

namespace Visual.Studio.Solution.Renamer.CommandLine
{
    public class CommandLineOptions
    {
        [Option('a', "apply", Required = false,
                HelpText               = "Disable preview mode and apply all the changes in the file system (Preview mode is enabled by default)")]
        public bool Apply { get; set; }

        public bool Preview => !Apply;

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

        [Option('m', "mask", Required = false, HelpText = "Set the list of file masks to be processed. Default is *.csproj *.cs *.xaml *.xml *.json")]
        public string Mask { get; set; }

        public string[] Masks => Mask.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        [Option('w', "workingdirectory", Required = false, HelpText = "Set working directory.")]
        public string WorkingDirectory { get; set; }
    }
}