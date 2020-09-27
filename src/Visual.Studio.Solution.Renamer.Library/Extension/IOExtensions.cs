using System;
using System.IO;

namespace Visual.Studio.Solution.Renamer.Library.Extension
{
    public static class IOExtensions
    {
        public static void AssertDirectoryExists(this string path)
        {
            if (!Directory.Exists(path)) throw new DirectoryNotFoundException(path);
        }

        public static void AssertFileExists(this string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(path);
        }

        public static string NormalizePath(this string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        public static string GetRelativePath(this string file, string folder)
        {
            Uri pathUri = new Uri(file);
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }

            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(
                folderUri
                    .MakeRelativeUri(pathUri)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar));
        }
    }
}