using System;
using Serilog;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    internal class FileUpdater
    {
        public void Run<TFile>(TFile[] files, Action<TFile> updateAction)
            where TFile : IFolderOrFile
        {
            Log.Verbose($"Found {files.Length} files");
            foreach (var file in files)
            {
                Log.Verbose($"Updating file {file.Name}");
                try
                {
                    updateAction(file);
                    Log.Verbose("Updated.");
                }
                catch (Exception e)
                {
                    Log.Error(e, "Failed.");
                }
            }
        }
    }
}