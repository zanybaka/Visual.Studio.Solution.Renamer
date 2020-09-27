using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Visual.Studio.Solution.Renamer.Library.Entity.Folder
{
    internal class FileFinder
    {
        public IEnumerable<FileInFileSystem> FindWithMask(string workingDirectory, string[] masks, bool recursively)
        {
            if (workingDirectory == null || masks == null || masks.Length == 0)
            {
                return Enumerable.Empty<FileInFileSystem>();
            }

            var entities = masks
                .SelectMany(m => Directory.EnumerateFiles(
                                workingDirectory,
                                m,
                                recursively
                                    ? SearchOption.AllDirectories
                                    : SearchOption.TopDirectoryOnly))
                .Select(d => new FileInFileSystem(d));

            return entities;
        }
    }
}