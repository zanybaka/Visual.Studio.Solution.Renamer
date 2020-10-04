using System;
using System.IO;
using Serilog;
using Visual.Studio.Solution.Renamer.Library.Builder;
using Visual.Studio.Solution.Renamer.Library.Entity.Folder;
using Visual.Studio.Solution.Renamer.Library.Task.Common;

namespace Visual.Studio.Solution.Renamer.Library.Task
{
    /// <summary>
    ///     Replaces file content
    /// </summary>
    internal class ReplaceContentInFileTask : IFolderOrFileTask
    {
        public bool Run(IFolderOrFile entity, UpdateOptions o)
        {
            UpdateFileOptions options = o as UpdateFileOptions;
            if (options == null)
            {
                throw new NotImplementedException($"Unsupported option '{o.GetType()}'");
            }

            if (options.From == null || options.To == null)
            {
                throw new ArgumentException("Set 'From' and 'To' properties for replacing content");
            }

            try
            {
                string content = File.ReadAllText(entity.AbsolutePath);
                if (content.Contains(options.From))
                {
                    Log.Verbose($"Replacing content in the file '{Path.GetFileName(entity.Name)}' from '{options.From}' to '{options.To}'");
                    if (!options.Preview)
                    {
                        File.WriteAllText(
                            entity.AbsolutePath,
                            content
                                .Replace(
                                    options.From,
                                    options.To,
                                    StringComparison.InvariantCultureIgnoreCase));
                    }

                    Log.Verbose("Replaced.");
                    return true;
                }

                Log.Verbose("No changes.");
                return false;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Can't replace content in the file '{entity.Name}'");
                return false;
            }
        }
    }
}