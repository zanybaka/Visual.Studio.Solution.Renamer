using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;
using Serilog;
using Visual.Studio.Solution.Renamer.Library;
using Visual.Studio.Solution.Renamer.Library.Entity;
using Visual.Studio.Solution.Renamer.Library.Extension;

namespace Visual.Studio.Solution.Renamer.CommandLine
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(Run)
                .WithNotParsed(OnWrongArguments);
        }

        private static void Run(CommandLineOptions options)
        {
            SetDefaults(options);

            InitializeLog(options);

            PrintArguments(options);

            if (!ValidateArguments(options)) return;

            try
            {
                Updates.Cleanup(options);

                UpdateFoldersAndFiles(options);
            }
            catch (Exception exception)
            {
                Log.Error(exception, messageTemplate: "Can't process the operation properly.");
            }

            PrintWarnings(options);
        }

        private static void UpdateFoldersAndFiles(CommandLineOptions options)
        {
            if (options.UseCsProj)
            {
                Updates.UpdateFoldersAndProjects(options);
            }
            else
            {
                if (!SolutionFile.Validate(options.SolutionFile))
                {
                    throw new NotSupportedException(
                        "Can't update the solution. Relative path with '..' is not supported. You can try to update with the argument --projects.");
                }

                Updates.UpdateFoldersProjectsAndSolution(options);
            }
        }

        private static bool ValidateArguments(CommandLineOptions options)
        {
            if (options.SolutionFile.IsEmpty() && !options.UseCsProj)
            {
                Log.Error(
                    "Solution file is not set. Multiple solutions are not supported. If you have .csproj files only try to set --projects argument.");
                return false;
            }

            return true;
        }

        private static void PrintArguments(CommandLineOptions options)
        {
            Log.Information("Options:");
            PropertyInfo[] properties = options.GetType().GetProperties();
            int            maxLength  = properties.Select(x => x.Name.Length).Max();
            foreach (PropertyInfo property in properties.Where(x => x.GetCustomAttribute<OptionAttribute>()?.Hidden == false))
            {
                object value = property.GetValue(options) ?? "(not set)";
                Log.Information($"\t{property.Name} {new string(' ', maxLength - property.Name.Length)}= {value}");
            }
        }

        private static void PrintWarnings(CommandLineOptions options)
        {
            if (options.Preview)
            {
                Log.Warning("Preview mode is ON.");
                Log.Warning("Please specify the argument --apply if you want to apply all the changes.");
            }

            if (options.SolutionFile.IsEmpty())
            {
                Log.Warning("Solution file needs to be updated manually as it was not set.");
            }
        }

        private static void SetDefaults(CommandLineOptions options)
        {
            options.WorkingDirectory ??= Environment.CurrentDirectory;
            options.WorkingDirectory =   options.WorkingDirectory.NormalizePath();
            options.Mask             ??= "*.csproj *.cs *.xaml *.xml *.json";
            if (options.SolutionFile.IsEmpty() && !options.UseCsProj)
            {
                string[] files = Directory.GetFiles(options.WorkingDirectory, "*.sln");
                if (files.Length == 1)
                {
                    options.SolutionFile = files.Single();
                }
            }

            if (options.SolutionFile.IsNotEmpty())
            {
                options.SolutionFile = options.SolutionFile.NormalizePath();
            }
        }

        private static void InitializeLog(CommandLineOptions options)
        {
            var logLevel = new LoggerConfiguration().MinimumLevel;

            Log.Logger =
                (options.Verbose || options.Preview
                    ? logLevel.Verbose()
                    : logLevel.Information())
                .WriteTo.Console()
                .CreateLogger();
        }

        private static void OnWrongArguments(IEnumerable<Error> errors)
        {
            foreach (Error error in errors)
            {
                Log.Error(error.ToString());
            }
        }
    }
}