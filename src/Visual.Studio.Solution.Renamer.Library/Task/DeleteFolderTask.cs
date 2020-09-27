using System;
using System.IO;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Deletes folder
    /// </summary>
    internal class DeleteFolderTask : IFolderOrFileTask
    {
        public bool Run(IFolderOrFile entity, UpdateOptions options)
        {
            try
            {
                Log.Verbose($"Deleting '{entity.AbsolutePath}'");
                if (!options.Preview)
                {
                    Directory.Delete(entity.AbsolutePath, recursive: true);
                }

                Log.Verbose("Deleted.");
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't delete folder {entity.Name}");
                return false;
            }

            return true;
        }
    }
}