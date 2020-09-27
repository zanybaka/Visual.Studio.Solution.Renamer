using System;
using Serilog;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    internal class FolderUpdater
    {
        public void Run<TFolder>(TFolder[] folders, Action<TFolder> updateAction)
            where TFolder : IFolderOrFile
        {
            Log.Verbose($"Found {folders.Length} folders");
            foreach (var folder in folders)
            {
                Log.Verbose($"Updating folder '{folder.Name}'");
                try
                {
                    updateAction(folder);
                }
                catch (Exception e)
                {
                    Log.Error(e, "FAILED");
                }
            }

            Log.Verbose("Updated.");
        }
    }
}