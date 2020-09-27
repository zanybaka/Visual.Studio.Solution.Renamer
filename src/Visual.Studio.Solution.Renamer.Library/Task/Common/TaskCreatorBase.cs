namespace Visual.Studio.Solution.Renamer.Library.Task.Common
{
    public abstract class TaskCreatorBase
    {
        internal abstract string Description { get; }

        internal string Path { get; set; }
    }
}