using System;
using System.IO;
using System.Linq;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Renames folder or file
    /// </summary>
    internal class RenameFolderOrFileTask : IFolderOrFileTask
    {
        public bool Run(IFolderOrFile entity, UpdateOptions o)
        {
            if (o is UpdateFolderOptions folderOptions)
            {
                if (folderOptions.Recursively)
                {
                    if (folderOptions.Masks.Length != 1)
                    {
                        throw new NotSupportedException("Please specify a single folder mask for recursive renaming.");
                    }

                    var subDirs = Directory.EnumerateDirectories(entity.AbsolutePath, folderOptions.Masks.Single());
                    if (subDirs.Any())
                    {
                        return false;
                    }

                    string newName = Path.Combine(Path.GetDirectoryName(entity.AbsolutePath),
                                                  Path.GetFileName(entity.AbsolutePath).Replace(folderOptions.From, folderOptions.To, StringComparison.InvariantCultureIgnoreCase));
                    if (entity.AbsolutePath.Equals(newName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return false;
                    }

                    Log.Verbose($"Renaming to '{newName}'");
                    if (!folderOptions.Preview)
                    {
                        Directory.Move(entity.AbsolutePath, newName);
                    }

                    return true;
                }

                throw new NotImplementedException("Folder non-recursive renaming is not implemented yet");
            }

            UpdateFileOptions options = o as UpdateFileOptions;
            if (options == null)
            {
                throw new NotImplementedException($"Unsupported option '{o.GetType()}'");
            }

            if (options.From == null || options.To == null)
            {
                throw new ArgumentException("Set 'From' and 'To' properties for renaming");
            }

            if (!entity.Name.Contains(options.From, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            try
            {
                string sourceFileName = entity.AbsolutePath;
                string destFileName   = Path.Combine(Path.GetDirectoryName(sourceFileName) ?? "", entity.Name.Replace(options.From, options.To, StringComparison.InvariantCultureIgnoreCase));
                if (sourceFileName.Equals(destFileName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }

                Log.Verbose($"Renaming to '{Path.GetFileName(destFileName)}'");
                if (!options.Preview)
                {
                    File.Move(sourceFileName, destFileName);
                }

                Log.Verbose("Renamed.");

                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't rename '{entity.Name}'");
                return false;
            }
        }
    }
}