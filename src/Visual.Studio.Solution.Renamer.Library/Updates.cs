using System;
using System.IO;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.Library
{
    public static class Updates
    {
        public static bool ValidateArguments(IRenameOptions options)
        {
            if (options.SolutionFile.IsEmpty() && !options.UseCsProj)
            {
                Log.Error(
                    "Solution file is not set. Multiple solutions are not supported. If you have .csproj files only try to set --projects argument.");
                return false;
            }

            return true;
        }

        public static void Cleanup(IRenameOptions options, Action onUpdate = null)
        {
            if (!options.Cleanup)
            {
                return;
            }

            Update
                .Folder.FilteredByName("bin", "obj")
                .Remove
                .At.Path(options.WorkingDirectory)
                .Folder.Recursively()
                .Itself
                .OnUpdate(onUpdate)
                .Run(options.Preview);
        }

        public static void UpdateFoldersProjectsAndSolution(IRenameOptions options, Action onUpdate = null)
        {
            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                if (options.RenameFoldersAndFiles)
                {
                    Update
                        .Project
                        .Name
                        .At.Path(options.WorkingDirectory)
                        .In
                        .Solution(options.SolutionFile)
                        .Replacing(options.ReplaceFrom, options.ReplaceTo)
                        .Names
                        .OnUpdate(onUpdate)
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
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }

                if (options.ReplaceFileContent)
                {
                    Update
                        .Folder
                        .ReplaceContent
                        .At.Path(options.WorkingDirectory)
                        .In
                        .File.FilteredByName(options.Masks)
                        .From(options.ReplaceFrom)
                        .To(options.ReplaceTo)
                        .Recursively()
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }
            }

            Update
                .Project
                .DefaultAssemblyName
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .OnUpdate(onUpdate)
                .Run(options.Preview);

            Update
                .Project
                .DefaultRootNamespace
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .OnUpdate(onUpdate)
                .Run(options.Preview);

            if (options.RenameFoldersAndFiles)
            {
                Update
                    .Folder
                    .Names
                    .At.Path(options.WorkingDirectory)
                    .With
                    .Project.DeclaredInSolution(options.SolutionFile)
                    .Names
                    .OnUpdate(onUpdate)
                    .Run(options.Preview);
            }

            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                if (options.RenameFoldersAndFiles)
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
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }

                if (options.ReplaceFileContent)
                {
                    Update
                        .Folder
                        .ReplaceContent
                        .At.Path(options.WorkingDirectory)
                        .In
                        .File.FilteredByName(Path.GetFileName(options.SolutionFile))
                        .From(options.ReplaceFrom)
                        .To(options.ReplaceTo)
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }

                if (options.RenameFoldersAndFiles)
                {
                    Update
                        .Folder
                        .ReplaceText
                        .At.Path(options.WorkingDirectory)
                        .In
                        .File.FilteredByName(Path.GetFileName(options.SolutionFile))
                        .From(options.ReplaceFrom)
                        .To(options.ReplaceTo)
                        .Names
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }
            }
        }

        public static void UpdateFoldersAndProjects(IRenameOptions options, Action onUpdate = null)
        {
            if (options.RenameFoldersAndFiles)
            {
                Update
                    .Folder
                    .Names
                    .At.Path(options.WorkingDirectory)
                    .With
                    .Project.All()
                    .Names
                    .OnUpdate(onUpdate)
                    .Run(options.Preview);
            }

            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                if (options.RenameFoldersAndFiles)
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
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }

                if (options.ReplaceFileContent)
                {
                    Update
                        .Folder
                        .ReplaceContent
                        .At.Path(options.WorkingDirectory)
                        .In
                        .File.FilteredByName(options.Masks)
                        .From(options.ReplaceFrom)
                        .To(options.ReplaceTo)
                        .Recursively()
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }
            }

            Update
                .Project
                .DefaultAssemblyName
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .OnUpdate(onUpdate)
                .Run(options.Preview);

            Update
                .Project
                .DefaultRootNamespace
                .At.Path(options.WorkingDirectory)
                .With
                .Project.FilteredByMask("*.csproj")
                .Names
                .OnUpdate(onUpdate)
                .Run(options.Preview);

            if (options.ReplaceFrom.IsNotEmpty() && options.ReplaceTo.IsNotEmpty())
            {
                if (options.RenameFoldersAndFiles)
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
                        .OnUpdate(onUpdate)
                        .Run(options.Preview);
                }
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