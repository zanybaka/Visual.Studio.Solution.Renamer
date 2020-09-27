using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    internal class FolderFinder
    {
        public IEnumerable<FolderInFileSystem> FindWithMask(string workingDirectory, string[] masks, bool recursively)
        {
            if (workingDirectory == null || masks == null || masks.Length == 0)
            {
                return Enumerable.Empty<FolderInFileSystem>();
            }

            var entities = masks
                .SelectMany(m => Directory.EnumerateDirectories(
                                workingDirectory,
                                m,
                                recursively
                                    ? SearchOption.AllDirectories
                                    : SearchOption.TopDirectoryOnly))
                .Select(d => new FolderInFileSystem(d));

            return entities;
        }
    }
}