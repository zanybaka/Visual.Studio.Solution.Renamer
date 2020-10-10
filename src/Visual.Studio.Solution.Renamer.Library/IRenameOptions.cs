namespace Visual.Studio.Solution.Renamer.Library
{
    public interface IRenameOptions
    {
        bool Preview { get; set; }

        bool RenameFoldersAndFiles { get; set; }
        
        bool ReplaceFileContent { get; set; }

        string ReplaceFrom { get; set; }

        string ReplaceTo { get; set; }

        bool Cleanup { get; set; }

        bool UseCsProj { get; set; }

        bool Verbose { get; set; }

        string SolutionFile { get; set; }

        string[] Masks { get; set; }

        string WorkingDirectory { get; set; }
    }
}