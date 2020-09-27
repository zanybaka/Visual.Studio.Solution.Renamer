using System;
using System.IO;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.CommandLine
{
    public static class Updates
    {
        public static void UpdateFoldersProjectsAndSolution(CommandLineOptions options)
        {
            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                Update
                    .Project
                    .Name
                    .At.Path(options.WorkingDirectory)
                    .In
                    .Solution(options.SolutionFile)
                    .Replacing(options.ReplaceFrom, options.ReplaceTo)
                    .Names
                    .Run(options.Preview);

                Update
                    .Folder
                    .ReplaceText
                    .At.Path(options.WorkingDirectory)
                    .In
                    .File.FilteredByName(options.Masks)
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Recursively()
                    .Run(options.Preview);

                Update
                    .Folder
                    .ReplaceContent
                    .At.Path(options.WorkingDirectory)
                    .In
                    .File.FilteredByName(options.Masks)
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Recursively()
                    .Run(options.Preview);
            }

            Update
                .Project
                .DefaultAssemblyName
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .Run(options.Preview);

            Update
                .Project
                .DefaultRootNamespace
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .Run(options.Preview);

            Update
                .Folder
                .Names
                .At.Path(options.WorkingDirectory)
                .With
                .Project.DeclaredInSolution(options.SolutionFile)
                .Names
                .Run(options.Preview);

            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                Update
                    .Folder
                    .ReplaceText
                    .At.Path(options.WorkingDirectory)
                    .In
                    .Folder.FilteredByName($"*{options.ReplaceFrom}*")
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Recursively()
                    .Run(options.Preview);

                Update
                    .Folder
                    .ReplaceContent
                    .At.Path(options.WorkingDirectory)
                    .In
                    .File.FilteredByName(Path.GetFileName(options.SolutionFile))
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Run(options.Preview);

                Update
                    .Folder
                    .ReplaceText
                    .At.Path(options.WorkingDirectory)
                    .In
                    .File.FilteredByName(Path.GetFileName(options.SolutionFile))
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Names
                    .Run(options.Preview);
            }
        }

        public static void UpdateFoldersAndProjects(CommandLineOptions options)
        {
            Update
                .Folder
                .Names
                .At.Path(options.WorkingDirectory)
                .With
                .Project.All()
                .Names
                .Run(options.Preview);

            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                Update
                    .Folder
                    .ReplaceText
                    .At.Path(options.WorkingDirectory)
                    .In
                    .File.FilteredByName(options.Masks)
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Recursively()
                    .Run(options.Preview);

                Update
                    .Folder
                    .ReplaceContent
                    .At.Path(options.WorkingDirectory)
                    .In
                    .File.FilteredByName(options.Masks)
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Recursively()
                    .Run(options.Preview);
            }

            Update
                .Project
                .DefaultAssemblyName
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .Run(options.Preview);

            Update
                .Project
                .DefaultRootNamespace
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .Run(options.Preview);

            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                Update
                    .Folder
                    .ReplaceText
                    .At.Path(options.WorkingDirectory)
                    .In
                    .Folder.FilteredByName($"*{options.ReplaceFrom}*")
                    .From(options.ReplaceFrom)
                    .To(options.ReplaceTo)
                    .Recursively()
                    .Run(options.Preview);
            }
        }

        internal static void Examples()
        {
            try
            {
                string solutionDirectory = @"c:\temp\1\";

                Update
                    .Folder
                    .ReplaceText
                    .At.Path(solutionDirectory)
                    .In
                    .File.FilteredByName("*.sln")
                    .From("Visual.Studio.Solution.Renamer")
                    .To("New.Name")
                    .Names
                    .Preview();

                Update
                    .Folder.FilteredByName("bin", "obj")
                    .Remove
                    .At.Path(solutionDirectory)
                    .Folder.Recursively()
                    .Itself
                    .Preview();

                Update
                    .Project
                    .Name
                    .At.Path(solutionDirectory)
                    .In
                    .Solution("New.Name.sln")
                    .Replacing(from: "Visual.Studio.Solution.Renamer", to: "New.Name")
                    .Names
                    .Preview();

                Update
                    .Folder
                    .ReplaceContent
                    .At.Path(solutionDirectory)
                    .In
                    .File.FilteredByName("*.csproj", "*.cs", "*.xaml")
                    .From("Visual.Studio.Solution.Renamer")
                    .To("New.Name")
                    .Recursively()
                    .Preview();

                Update
                    .Folder
                    .ReplaceText
                    .At.Path(solutionDirectory)
                    .In
                    .File.FilteredByName("*.csproj", "*.cs")
                    .From("Visual.Studio.Solution.Renamer")
                    .To("New.Name")
                    .Recursively()
                    .Preview();

                Update
                    .Folder
                    .Names
                    .At.Path(solutionDirectory)
                    .With
                    .Project.DeclaredInSolution("New.Name.sln")
                    .Names
                    .Preview();

                Update
                    .Folder
                    .Names
                    .At.Path(solutionDirectory)
                    .With
                    .Project.All()
                    .Names
                    .Preview();

                Update
                    .Folder
                    .Names
                    .At.Path(solutionDirectory)
                    .With
                    .Project.FilteredByMask("*.csproj")
                    .Names
                    .Preview();

                Update
                    .Project
                    .DefaultAssemblyName
                    .At.Path(solutionDirectory)
                    .With
                    .Project.FilteredByMask("*.csproj")
                    .Names
                    .Preview();

                Update
                    .Project
                    .DefaultRootNamespace
                    .At.Path(solutionDirectory)
                    .With
                    .Project.FilteredByMask("*.csproj")
                    .Names
                    .Preview();
            }
            catch (Exception exception)
            {
                Log.Error(exception, messageTemplate: "Can't process the operation properly.");
            }
        }
    }
}